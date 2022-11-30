using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// MusicPaseが発行するテンポに合わせてAudioSourceのpitchを調節するクラス
/// </summary>
public class ResultSceneController : MonoBehaviour {

    /// <summary>
    /// スコアを表示する時の効果音を鳴らすAudioSource
    /// </summary>
    [SerializeField] protected AudioSource scoreAppearanceSource;

    /// <summary>
    /// スタンディングオベーションを鳴らすAudioSource
    /// </summary>
    [SerializeField] protected AudioSource standingOvationSource;

    /// <summary>
    /// スコア表示SEのファイル名。拡張子は無しで日本語は使用しない
    /// </summary>
    [Header("スコア表示SEのファイル名。拡張子は無しで日本語は使用しない")]
    [SerializeField] protected string scoreAppearanceSEName;

    /// <summary>
    /// スタンディングオベーションSEのファイル名。拡張子は無しで日本語は使用しない
    /// </summary>
    [Header("スタンディングオベーションSEのファイル名。拡張子は無しで日本語は使用しない")]
    [SerializeField] protected string standingOvationSEName;

    /// <summary>
    /// スコアを表示するテキスト。
    /// </summary>
    [Header("スコアを表示するテキスト。")]
    [SerializeField] Text scoreText;

    /// <summary>
    /// リザルトを始める。
    /// </summary>
    protected new void Start(){
        scoreAppearanceSource.clip = Resources.Load<AudioClip>(scoreAppearanceSEName);
        standingOvationSource.clip = Resources.Load<AudioClip>(standingOvationSEName);

        //音楽をフェードするコルーチンを止める。
        //フェードのタイミングはTutorialCoroutineから指定する。
        StartCoroutine(ResultCoroutine());
    }

    /// <summary>
    /// SEとともに結果を表示し、デモシーン状態に戻る。
    /// </summary>
    protected IEnumerator ResultCoroutine()
    {
        scoreText.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        scoreAppearanceSource.Play();
        scoreText.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        standingOvationSource.Play();

        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("DemoScene");
    }
}
