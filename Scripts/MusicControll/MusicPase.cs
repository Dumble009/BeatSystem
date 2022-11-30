using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// テンポが変化した時に発行するイベント
/// </summary>
/// <param name="normalizedTempo">正規化された現在のテンポ。本来の曲のテンポと一致している場合は1。倍速の場合は2</param>
public delegate void TempoChangeHandler(float normalizedTempo);

/// <summary>
/// テンポが刻まれた時に発行するイベント（2種類ある）
/// </summary>
public delegate void TimingHandler();

/// <summary>
/// テンポに応じてAudioSourceの再生速度を変える
/// </summary>
public class MusicPase : MonoBehaviour
{
    [SerializeField] BeatMakerHolder holder;
    /// <summary>
    /// テンポのずれがこの閾値以下であればORIGINAL_TEMPOとして扱う
    /// </summary>
    [Header("閾値 大きくすればするほど大きなずれを正常なものとして許容する")]
    [SerializeField] float threshold = 0.1f;

    /// <summary>
    /// テンポが1秒間に変化する量
    /// </summary>
    [Header("テンポが1秒間に変化する量 大きくすればテンポが速く変化するようになる")]
    [SerializeField] float tempoChangeSpeed = 10;

    /// <summary>
    /// 移動平均を計算するために使用するサンプル数
    /// </summary>
    [Header("何回分の移動平均を取るか 最低値は1 小さくすると直近のテンポのずれを素早く反映する")]
    [SerializeField] int windowSize = 3;

    /// <summary>
    /// BGMのBPM。「タップテンポはかるくん」などで計測した値を入力する
    /// </summary>
    [Header("BGMのBPM。「タップテンポはかるくん」などで計測した値を入力する")]
    [SerializeField] float originalBPM;

    float originalTempo;
    public float OriginalTempo{
        get {return originalTempo;}
    }

    /// <summary>
    /// 移動平均を計算するために使用する過去の拍動間隔のキュー
    /// </summary>
    Queue<float> tempoQ;

    /// <summary>
    /// 最後にBeatMakerから送られてきたテンポ
    /// </summary>
    float lastBeatedTempo = 0.0f;

    /// <summary>
    /// 現在の音楽のテンポ
    /// </summary>
    float currentTempo = 0.0f;

    /// <summary>
    /// テンポ変化時に発行するイベント。テストケースでモッククラスが呼び出せるようにprotectedにしておく
    /// </summary>
    protected TempoChangeHandler onTempoChange;
    
    /// <summary>
    /// テンポが正しく刻まれた時に発行するイベント。
    /// </summary>
    protected TimingHandler onJustTiming;

    /// <summary>
    /// テンポが間違って刻まれた時に発行するイベント。
    /// </summary>
    protected TimingHandler onOutOfTiming;

    float lastBeatTime = 0.0f;

    private void Awake()
    {
        tempoQ = new Queue<float>();
        originalTempo = (float) 60.0 / originalBPM;

        // イベントを空関数で初期化しておき、nullを防ぐ
        onTempoChange = (x) => { };
        onJustTiming = () => { };
        onOutOfTiming = () => { };
    }

    private void Start()
    {
        if (holder != null)
        {
            holder.RegisterOnBeat(OnBeat);
        }
        else
        {
            Debug.LogError("BeatMakerHolder holder is null");
        }
    }

    private void Update()
    {
        float currentTargetTempo = NormalizeRealTimeTempo(CalculateTargetTempo(lastBeatedTempo));
        //Debug.Log(currentTargetTempo);
        currentTempo = CalculateCurrentTempo(currentTargetTempo);
        onTempoChange(currentTempo);
    }

    private void OnBeat(BeatPacket packet)
    {
        lastBeatTime = Time.realtimeSinceStartup;
        lastBeatedTempo = packet.Tempo;

        tempoQ.Enqueue(packet.Tempo);
        if (tempoQ.Count > windowSize)
        {
            tempoQ.Dequeue();
        }

        if(tempoQ.Count > 0 && currentTempo == 1.0){
            onJustTiming();
        } else {
            onOutOfTiming();
        }
    }

    /// <summary>
    /// 目標テンポを元に現在のテンポを返す。取り扱うテンポは全て正規化されたもの
    /// </summary>
    /// <param name="normalizedTargetTempo">正規化された目標テンポ</param>
    /// <returns>正規化された現在のテンポ</returns>
    float CalculateCurrentTempo(float normalizedTargetTempo)
    {
        if (Mathf.Abs(currentTempo - normalizedTargetTempo) <= threshold)
        {
            return normalizedTargetTempo;
        }
        else
        {
            var temp = currentTempo;
            var delta = tempoChangeSpeed * Time.deltaTime;
            temp += (currentTempo < normalizedTargetTempo) ? delta : -delta;

            return temp;
        }
    }

    /// <summary>
    /// 目標テンポを実時間で計算して返す
    /// </summary>
    /// <param name="currentTempo">最後に打たれたビートのテンポ</param>
    /// <returns>目標テンポ。単位は次の拍までの秒数</returns>
    float CalculateTargetTempo(float currentTempo)
    {
        if (tempoQ.Count == 0)
        {
            return originalTempo;
        }
        else
        {
            float average = 0;
            foreach (var tempo in tempoQ)
            {
                average += tempo;
            }

            float currentTimeFromLastBeat = Time.realtimeSinceStartup - lastBeatTime;

            if (currentTimeFromLastBeat >= currentTempo + threshold)
            {
                average += currentTimeFromLastBeat;
                average /= tempoQ.Count + 1;
            }
            else
            {
                average /= tempoQ.Count;
            }

            if (Mathf.Approximately(RoundByThreshold(currentTempo, originalTempo), originalTempo))
            {
                average = RoundByThreshold(average, originalTempo);
            }

            return average;
        }
    }

    /// <summary>
    /// 拍動間の秒数で表されるテンポをオリジナルのテンポからの割合で表現されるテンポへと変換する
    /// </summary>
    /// <param name="realTimeTempo">変換対象のテンポ。拍動間の秒数</param>
    /// <returns>realTimeTempoを正規化したテンポ</returns>
    private float NormalizeRealTimeTempo(float realTimeTempo)
    {
        return originalTempo / realTimeTempo;
    }

    /// <summary>
    /// floatの値valとtargetの差がthreshold以下であればtargetを、そうでなければvalを返す。
    /// </summary>
    /// <param name="val">丸める対象の値</param>
    /// <param name="target">丸めの基準となる値</param>
    /// <returns>必要に応じて丸められたval</returns>
    private float RoundByThreshold(float val, float target)
    {
        if (Mathf.Abs(1/target - 1/val) <= threshold)
        {
            return target;
        }
        else
        {
            return val;
        }
    }

    /// <summary>
    /// 現在のテンポを変化させるコルーチン
    /// </summary>
    private IEnumerator TempoChange(float currentTempo)
    {
        while (true)
        {
            // 最初の拍動があるまではオリジナルテンポを出し続ける
            if (tempoQ.Count == 0)
            {
                currentTempo = originalTempo;
            }
            else
            {
                float average = 0;
                foreach (var tempo in tempoQ)
                {
                    average += tempo;
                }

                average /= tempoQ.Count;

            }
            yield return null;
        }
    }

    /// <summary>
    /// テンポが変化した際のイベントにメッセージを登録する。
    /// </summary>
    /// <param name="e">テンポが変化した際に呼び出される処理</param>
    public void RegisterOnTempoChange(TempoChangeHandler e)
    {
        onTempoChange += e;
    }

    /// <summary>
    /// テンポが正しく刻まれた際のイベントにメッセージを登録する。
    /// </summary>
    /// <param name="e">テンポが変化した際に呼び出される処理</param>
    public void RegisterOnJustTiming(TimingHandler e)
    {
        onJustTiming += e;
        Debug.LogError(e);
    }

    /// <summary>
    /// テンポが間違って刻まれた際のイベントにメッセージを登録する。
    /// </summary>
    /// <param name="e">テンポが変化した際に呼び出される処理</param>
    public void RegisterOnOutOfTiming(TimingHandler e)
    {
        onOutOfTiming += e;
    }
}
