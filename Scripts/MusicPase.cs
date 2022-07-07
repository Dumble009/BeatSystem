using System.Collections;
using UnityEngine;
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

    private void Start()
    {
        holder.RegisterOnBeat(OnBeat);
    }

    private void OnBeat(BeatPacket packet)
    {
        currentTargetTempo = packet.Tempo;
        if (Mathf.Abs(currentTargetTempo - ORIGINAL_TEMPO) <= threshold)
        {
            currentTargetTempo = ORIGINAL_TEMPO;
        }
    }

    IEnumerator TempoChange()
    {
        yield return null;
    }
}
