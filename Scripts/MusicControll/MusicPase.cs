using System.Collections;
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
    [SerializeField] AudioSource audioSource;
    [SerializeField] BeatMakerHolder holder;
    /// <summary>
    /// テンポのずれがこの閾値以下であればORIGINAL_TEMPOとして扱う
    /// </summary>
    [SerializeField] float threshold = 0.1f;

    /// <summary>
    /// テンポが1秒間に変化する量
    /// </summary>
    [SerializeField] float tempoChangeSpeed = 10;

    const float ORIGINAL_TEMPO = 0.56f;

    /// <summary>
    /// 現在の目標テンポ
    /// </summary>
    float currentTargetTempo = 0.0f;

    /// <summary>
    /// 現在の音楽のテンポ
    /// </summary>
    float currentTempo = 0.0f;

    /// <summary>
    /// テンポ変化時に発行するイベント。テストケースでモッククラスが呼び出せるようにprotectedにしておく
    /// </summary>
    protected TempoChangeHandler onTempoChange;

    private void Awake()
    {
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

    private void OnBeat(BeatPacket packet)
    {
        currentTargetTempo = packet.Tempo;
        if (Mathf.Abs(currentTargetTempo - ORIGINAL_TEMPO) <= threshold)
        {
            currentTargetTempo = ORIGINAL_TEMPO;
        }

        // WARN:今は簡単にするためにこのタイミングでイベントを発行するが、本来はTempoChangeで発行する。
        onTempoChange(currentTargetTempo);
    }

    private IEnumerator TempoChange()
    {
        yield return null;
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
