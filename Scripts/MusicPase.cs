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

    const float ORIGINAL_TEMPO = 0.56f;

    private void Start() {
        holder.RegisterOnBeat(OnBeat);
    }

    private void OnBeat(BeatPacket packet){
        float tempo = packet.Tempo;
        if(Mathf.Abs(tempo - ORIGINAL_TEMPO) <= threshold){
            tempo = ORIGINAL_TEMPO;
        }
        audioSource.pitch = ORIGINAL_TEMPO / tempo;
    }
}
