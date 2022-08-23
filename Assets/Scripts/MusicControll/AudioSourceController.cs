using System;
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
    /// BGMをラジオ音源風にするローパスフィルター
    /// </summary>
    [SerializeField] AudioLowPassFilter mainBGMLowPassFilter;

    /// <summary>
    /// 体験終了後の拍手を鳴らすAudioSource
    /// </summary>
    [SerializeField] AudioSource clappingSource;

    /// <summary>
    /// テンポを正しく刻んだときの音を鳴らすAudioSource
    /// </summary>
    [SerializeField] AudioSource justTimingSoundSource;

    /// <summary>
    /// テンポが早すぎるときの警告音を鳴らすAudioSource
    /// </summary>
    [SerializeField] AudioSource warningSoundSource;

    /// <summary>
    /// BGMのファイル名。拡張子は無しで日本語は使用しない
    /// </summary>
    [Header("BGMのファイル名。拡張子は無しで日本語は使用しない")]
    [SerializeField] string mainBGMClipName;

    /// <summary>
    /// 拍手のSEのファイル名。拡張子は無しで日本語は使用しない
    /// </summary>
    [Header("拍手のSEのファイル名。拡張子は無しで日本語は使用しない")]
    [SerializeField] string clappingSEName;

    /// ジャストタイミングSEのファイル名。拡張子は無しで日本語は使用しない
    /// </summary>
    [Header("ジャストタイミングSEのファイル名。拡張子は無しで日本語は使用しない")]
    [SerializeField] string justTimingSoundName;

    /// <summary>
    /// 拍手のSEのファイル名。拡張子は無しで日本語は使用しない
    /// </summary>
    [Header("警告音のSEのファイル名。拡張子は無しで日本語は使用しない")]
    [SerializeField] string warningSoundName;

    /// <summary>
    /// 体験時間。この時間が経過したらフェードアウトが始まる。
    /// </summary>
    [Header("体験時間の秒数。この秒数経過したらフェードアウトが始まる")]
    [SerializeField] float playSeconds = 30.0f;

    /// <summary>
    /// フェードアウトにかかる時間。この時間立つと音が消失する
    /// </summary>
    [Header("フェードアウトにかかる秒数。フェードアウトが始まってからこの秒数経過すると音が消失する")]
    [SerializeField] float fadeTime = 1.0f;

    /// <summary>
    /// フェードアウト中かどうか。これがtrueだとOnTempoChangeが働きを失う
    /// </summary>
    bool isFading = false;


    private void Start()
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

        if (clappingSource != null)
        {
            clappingSource.clip = Resources.Load<AudioClip>(clappingSEName);
        }

        if (justTimingSoundSource != null)
        {
            justTimingSoundSource.clip = Resources.Load<AudioClip>(justTimingSoundName);
        }

        if (warningSoundSource != null)
        {
            warningSoundSource.clip = Resources.Load<AudioClip>(warningSoundName);
            warningSoundSource.volume = 0.0f;
            warningSoundSource.loop = true;
            warningSoundSource.Play();
        }

        StartCoroutine(FadeCoroutine());
    }

    /// <summary>
    /// MusicPaseでテンポの変化が起こった際に呼び出されるイベント

    /// </summary>
    /// <param name="normalizedTempo">通常のテンポが1、倍速だと2と正規化されたテンポ</param>
    private void OnTempoChange(float normalizedTempo)
    {
        //フェードアウト中なら実行しない
        if(isFading) return;

        //受け取った（正規化）テンポが1より小さければ、テンポに比例してBGMにリバーブがかかる
        //リバーブを掛けたBGMを音量0で流し
        //通常BGMとクロスフェード（片方はフェードイン、もう片方はフェードアウト）することで
        //強引にリバーブしているように聞こえさせる
        if(normalizedTempo < 1)
        {
            mainBGMLowPassFilter.cutoffFrequency = 1000;
        }
        else
        {
            mainBGMLowPassFilter.cutoffFrequency = 22000;
        }
        
        if(1 < normalizedTempo)
        {
            warningSoundSource.volume = 1.0f;
        }
        else
        {
            warningSoundSource.volume = 0.0f;
        }
    }

    /// <summary>
    /// 体験時間だけ待って、その後フェードを始める。
    /// </summary>
    IEnumerator FadeCoroutine()
    {
        yield return new WaitForSeconds(playSeconds);

        // ここからフェードが始まる
        isFading = true;

        //フェードアウト時に、フェードアウト開始時点の音量を参照する
        float lastVolume = mainBGMSource.volume;

        for (float i = 0.0f; i <= fadeTime; i += Time.deltaTime)
        {
            mainBGMSource.volume = lastVolume * (fadeTime - i) / fadeTime;
            yield return null;
        }

        mainBGMSource.volume = 0.0f;

        if (clappingSource != null)
        {
            clappingSource.Play();
        }
    }

    void OnJustTiming()
    {
        justTimingSoundSource.volume = 0.2f;
        justTimingSoundSource.Play();
    }
}
