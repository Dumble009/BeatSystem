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
    [SerializeField] protected AudioSource mainBGMSource;

    /// <summary>
    /// BGMの音質を落とすAudioLowPassFilter
    /// </summary>
    [SerializeField] protected AudioLowPassFilter mainBGMLowPassFilter;

    /// <summary>
    /// テンポが正しく刻まれた時の効果音を鳴らすAudioSource
    /// </summary>
    [SerializeField] protected AudioSource justTimingSource;

    /// <summary>
    /// 体験終了後の拍手を鳴らすAudioSource
    /// </summary>
    [SerializeField] protected AudioSource clappingSource;

    /// <summary>
    /// 体験終了後の拍手を鳴らすAudioSource
    /// </summary>
    [SerializeField] protected AudioSource warningSource;

    /// <summary>
    /// BGMのファイル名。拡張子は無しで日本語は使用しない
    /// </summary>
    [Header("BGMのファイル名。拡張子は無しで日本語は使用しない")]
    [SerializeField] protected string mainBGMClipName;

    /// <summary>
    /// ジャストタイミングSEのファイル名。拡張子は無しで日本語は使用しない
    /// </summary>
    [Header("BGMのファイル名。拡張子は無しで日本語は使用しない")]
    [SerializeField] protected string justTimingSEName;

    /// <summary>
    /// 拍手のSEのファイル名。拡張子は無しで日本語は使用しない
    /// </summary>
    [Header("拍手のSEのファイル名。拡張子は無しで日本語は使用しない")]
    [SerializeField] protected string clappingSEName;

    /// <summary>
    /// 警告音のSEのファイル名。拡張子は無しで日本語は使用しない
    /// </summary>
    [Header("拍手のSEのファイル名。拡張子は無しで日本語は使用しない")]
    [SerializeField] protected string warningSEName;

    /// <summary>
    /// 体験時間。この時間が経過したらフェードアウトが始まる。
    /// </summary>
    [Header("体験時間の秒数。この秒数経過したらフェードアウトが始まる")]
    [SerializeField] protected float playSeconds = 10.0f;

    /// <summary>
    /// フェードアウトにかかる時間。この時間立つと音が消失する
    /// </summary>
    [Header("フェードアウトにかかる秒数。フェードアウトが始まってからこの秒数経過すると音が消失する")]
    [SerializeField] protected float fadeTime = 1.0f;

    /// <summary>
    /// フェードアウトを行うコルーチン。
    /// 派生クラスから一時停止するため、情報を保持しておく。
    /// </summary>
    protected IEnumerator fadeCoroutine;

    protected void Start()
    {
        var m = FindObjectOfType<MusicPase>();
        if (m != null)
        {
            m.RegisterOnTempoChange(this.OnTempoChange);
            m.RegisterOnJustTiming(this.OnJustTiming);
        }
        else
        {
            Debug.LogError("There isn't MusicPase Component.");
        }

        mainBGMSource.clip = Resources.Load<AudioClip>(mainBGMClipName);
        mainBGMSource.Play();

        if (justTimingSource != null)
        {
            justTimingSource.clip = Resources.Load<AudioClip>(justTimingSEName);
            justTimingSource.volume = 0.4f;
        }

        if (clappingSource != null)
        {
            clappingSource.clip = Resources.Load<AudioClip>(clappingSEName);
        }

        if (warningSource != null)
        {
            warningSource.clip = Resources.Load<AudioClip>(warningSEName);
            warningSource.volume = 0.0f;
            warningSource.Play();
        }

        fadeCoroutine = FadeCoroutine();
        StartCoroutine(fadeCoroutine);
    }

    /// <summary>
    /// MusicPaseでテンポの変化が起こった際に呼び出されるイベント
    /// </summary>
    /// <param name="normalizedTempo">通常のテンポが1、倍速だと2と正規化されたテンポ</param>
    protected void OnTempoChange(float normalizedTempo)
    {
        //Debug.Log(normalizedTempo);

        if (normalizedTempo < 1.0)
        {
            mainBGMLowPassFilter.cutoffFrequency = 500;
        }
        else
        {
            mainBGMLowPassFilter.cutoffFrequency = 22000;
        }

        if (1.0 < normalizedTempo)
        {
            warningSource.volume = 1.0f;
        }
        else
        {
            warningSource.volume = 0.0f;
        }
    }

    /// <summary>
    /// MusicPaseでテンポが正しく刻まれた際に呼び出されるイベント
    /// </summary>
    protected void OnJustTiming()
    {
        justTimingSource.Play();
    }

    /// <summary>
    /// 体験時間だけ待って、その後フェードを始める。
    /// </summary>
    protected IEnumerator FadeCoroutine()
    {
        yield return new WaitForSeconds(playSeconds);

        // ここからフェードが始まる
        for (float i = 0.0f; i <= fadeTime; i += Time.deltaTime)
        {
            mainBGMSource.volume = (fadeTime - i) / fadeTime;
            yield return null;
        }

        mainBGMSource.volume = 0.0f;

        if (clappingSource != null)
        {
            clappingSource.Play();
            // ここからフェードが始まる
            for (float i = 0.0f; i <= fadeTime; i += Time.deltaTime)
            {
                clappingSource.volume = (fadeTime - i) / fadeTime;
                yield return null;
            }
        }
    }
}
