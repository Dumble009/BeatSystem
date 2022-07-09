using UnityEngine;

/// <summary>
/// MusicPaseが発行するテンポに合わせてAudioSourceのpitchを調節するクラス
/// </summary>
public class AudioSourceController : MonoBehaviour
{
    /// <summary>
    /// 操作対象のAudioSource
    /// </summary>
    AudioSource targetSource;

    private void Awake()
    {
        targetSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        var m = FindObjectOfType<MusicPase>();
        if (m != null)
        {
            m.RegisterOnTempoChange(this.OnTempoChange);
        }
        else
        {
            Debug.LogError("There isn't MusicPase Component.");
        }
    }

    /// <summary>
    /// MusicPaseでテンポの変化が起こった際に呼び出されるイベント
    /// </summary>
    /// <param name="normalizedTempo">通常のテンポが1、倍速だと2と正規化されたテンポ</param>
    private void OnTempoChange(float normalizedTempo)
    {
        targetSource.pitch = normalizedTempo;
    }
}
