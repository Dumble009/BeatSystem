using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// MusicPaseが発行するテンポに合わせてAudioSourceのpitchを調節するクラス
/// </summary>
public class MusicSceneController : AudioSourceController {

    /// <summary>
    /// カウントダウンを行うオブジェクト。
    /// </summary>
    [Header("カウントダウンを行うオブジェクト。")]
    [SerializeField] protected GameObject countDown;

    /// <summary>
    /// カウントダウンの数字。3つある。
    /// </summary>
    [Header("準備時間の秒数。この秒数が経過したら音楽が始まる")]
    [SerializeField] protected TextMeshProUGUI countDownText;

    /// <summary>
    /// リザルト画面の表示時間。音楽がフェードアウト後、この時間だけリザルトシーンに行く前に猶予が生じる
    /// </summary>
    [Header("リザルト画面の表示時間。音楽がフェードアウト後、この時間だけリザルトシーンに行く前に猶予が生じる")]
    [SerializeField] protected float durationForResult;

    /// <summary>
    /// デモを始める。
    /// </summary>
    protected new void Start(){
        base.Start();

        mainBGMSource.Pause();

        //音楽をフェードするコルーチンを止める。
        //フェードのタイミングはTutorialCoroutineから指定する。
        StopCoroutine(fadeCoroutine);
        StartCoroutine(MusicSceneCoroutine());
    }

    /// <summary>
    /// チュートリアルを行う。
    /// その後、フェードやデモの再読み込みのコルーチンを開始する。
    /// </summary>
    protected IEnumerator MusicSceneCoroutine()
    {
        //カウントダウン入れる
        countDown.gameObject.SetActive(true);

        for (int i = 3; i > 0 ; i--)
        {
            countDownText.text = $"{i}";
            yield return new WaitForSeconds(1);
        }

        countDown.gameObject.SetActive(false);
        warningSource.enabled = true;

        mainBGMSource.Play();
        StartCoroutine(FadeCoroutine());
        StartCoroutine(Go2ResultCoroutine());

        yield return null;
    }

    /// <summary>
    /// デモが終わるまで待って、すこし経つとまたデモを始める。
    /// </summary>
    protected IEnumerator Go2ResultCoroutine()
    {
        yield return new WaitForSeconds(playSeconds + fadeTime);
        yield return new WaitForSeconds(durationForResult);
        SceneManager.LoadScene("ResultScene");
    }
}
