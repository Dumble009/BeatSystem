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
    /// リザルト画面の表示時間。音楽がフェードアウト後、この時間だけリザルトが流れる
    /// </summary>
    [Header("リザルト画面の表示時間。音楽がフェードアウト後、この時間だけリザルトが流れる")]
    [SerializeField] protected float resultTimeSeconds;

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

        mainBGMSource.Play();
        StartCoroutine(FadeCoroutine());
        StartCoroutine(Return2DemoCoroutine());

        yield return null;
    }

    /// <summary>
    /// デモが終わるまで待って、すこし経つとまたデモを始める。
    /// </summary>
    protected IEnumerator Return2DemoCoroutine()
    {
        yield return new WaitForSeconds(playSeconds + fadeTime);
        StartCoroutine(ResultCoroutine());
        yield return new WaitForSeconds(resultTimeSeconds);

        SceneManager.LoadScene("DemoScene");
    }

    /// <summary>
    /// リザルトを表示する。
    /// </summary>
    protected IEnumerator ResultCoroutine()
    {
        //未実装
        yield return null;
    }
}
