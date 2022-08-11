using System.Collections;
using UnityEngine;

/// <summary>
/// MusicPaseが発行するテンポに合わせてAudioSourceのpitchを調節するクラス
/// </summary>
public class AudioSourceController : MonoBehaviour
{
    /// <summary>
    /// BGMを鳴らすAudioSource
    /// </summary>
    [SerializeField] AudioSource mainBGMSource;

    /// <summary>
    /// 体験終了後の拍手を鳴らすAudioSource
    /// </summary>
    [SerializeField] AudioSource clappingSource;

    /// <summary>
    /// BGMのファイル名。拡張子は無しで日本語は使用しない
    /// </summary>
    [SerializeField] string mainBGMClipName;

    /// <summary>
    /// 拍手のSEのファイル名。拡張子は無しで日本語は使用しない
    /// </summary>
    [SerializeField] string clappingSEName;

    /// <summary>
    /// 体験時間。この時間が経過したらフェードアウトが始まる。
    /// </summary>
    [SerializeField] float playSeconds = 10.0f;

    /// <summary>
    /// フェードアウトにかかる時間。この時間立つと音が消失する
    /// </summary>
    [SerializeField] float fadeTime = 1.0f;

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

        mainBGMSource.clip = Resources.Load<AudioClip>(mainBGMClipName);
        mainBGMSource.Play();
        
        if(clappingSource != null){
            clappingSource.clip = Resources.Load<AudioClip>(clappingSEName);
        }

        StartCoroutine(FadeCoroutine());
    }

    /// <summary>
    /// MusicPaseでテンポの変化が起こった際に呼び出されるイベント
    /// </summary>
    /// <param name="normalizedTempo">通常のテンポが1、倍速だと2と正規化されたテンポ</param>
    private void OnTempoChange(float normalizedTempo)
    {
        mainBGMSource.pitch = normalizedTempo;
    }

    /// <summary>
    /// 体験時間だけ待って、その後フェードを始める。
    /// </summary>
    IEnumerator FadeCoroutine(){
        yield return new WaitForSeconds(playSeconds);

        // ここからフェードが始まる
        for (float i = 0.0f; i <= fadeTime; i += Time.deltaTime){
            mainBGMSource.volume = (fadeTime - i) / fadeTime;
            yield return null;
        }

        mainBGMSource.volume = 0.0f;

        if(clappingSource != null){
            clappingSource.Play();
        }
    }
}
