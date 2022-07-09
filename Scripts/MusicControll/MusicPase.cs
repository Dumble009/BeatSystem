using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// テンポが変化した時に発行するイベント
/// </summary>
/// <param name="normalizedTempo">正規化された現在のテンポ。本来の曲のテンポと一致している場合は1。倍速の場合は2</param>
public delegate void TempoChangeHandler(float normalizedTempo);
/// <summary>
/// テンポに応じてAudioSourceの再生速度を変える
/// </summary>
public class MusicPase : MonoBehaviour
{
    [SerializeField] BeatMakerHolder holder;
    /// <summary>
    /// テンポのずれがこの閾値以下であればORIGINAL_TEMPOとして扱う
    /// </summary>
    [SerializeField] float threshold = 0.1f;

    /// <summary>
    /// テンポが1秒間に変化する量
    /// </summary>
    [SerializeField] float tempoChangeSpeed = 10;

    /// <summary>
    /// 移動平均を計算するために使用するサンプル数
    /// </summary>
    [SerializeField] int windowSize = 3;

    const float ORIGINAL_TEMPO = 0.56f;

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

    float lastBeatTime = 0.0f;

    private void Awake()
    {
        tempoQ = new Queue<float>();

        // イベントを空関数で初期化しておき、nullを防ぐ
        onTempoChange = (x) => { };
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
        float currentTargetTempo = CalculateTargetTempo(lastBeatedTempo);
        currentTempo = CalcCurrentTempo(currentTargetTempo);
        onTempoChange(ORIGINAL_TEMPO / currentTempo);
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
        /*lastBeatTime = Time.realtimeSinceStartup;

        currentTargetTempo = packet.Tempo;
        if (Mathf.Abs(currentTargetTempo - ORIGINAL_TEMPO) <= threshold)
        {
            currentTargetTempo = ORIGINAL_TEMPO;
        }

        // WARN:今は簡単にするためにこのタイミングでイベントを発行するが、本来はTempoChangeで発行する。
        onTempoChange(ORIGINAL_TEMPO / currentTargetTempo);*/
    }

    float CalcCurrentTempo(float targetTempo)
    {
        if (Mathf.Abs(currentTempo - targetTempo) <= threshold)
        {
            return targetTempo;
        }
        else
        {
            var temp = currentTempo;
            var delta = tempoChangeSpeed * Time.deltaTime;
            temp += (currentTempo < targetTempo) ? delta : -delta;

            return temp;
        }
    }

    float CalculateTargetTempo(float currentTempo)
    {
        if (tempoQ.Count == 0)
        {
            return ORIGINAL_TEMPO;
        }
        else
        {
            float average = 0;
            foreach (var tempo in tempoQ)
            {
                average += tempo;
            }

            if (Time.realtimeSinceStartup - lastBeatTime >= currentTempo + threshold)
            {
                average += Time.realtimeSinceStartup - lastBeatTime;
                average /= tempoQ.Count + 1;
            }
            else
            {
                average /= tempoQ.Count;
            }



            return average;
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
                currentTempo = ORIGINAL_TEMPO;
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
}
