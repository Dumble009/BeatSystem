using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// チュートリアルの画像をフェードイン・フェードアウトさせるイベント。
/// </summary>
public delegate void fadeTutorial(int imageNum, float fadeTime);

/// <summary>
/// MusicPaseが発行するテンポに合わせてAudioSourceのpitchを調節するクラス
/// </summary>
public class DemoSceneController : AudioSourceController {

    /// <summary>
    /// チュートリアルの秒数。
    /// この秒数が経過したら、デモが始まる
    /// </summary>
    [Header("チュートリアルの秒数。この秒数が経過したらデモが始まる")]
    [SerializeField] protected float tutorialSeconds;

    /// <summary>
    /// デモが終わり、再読み込みされるまでの秒数。
    /// デモ終了後、この秒数が経過したら再びデモが始まる。
    /// </summary>
    [Header("デモ終了後、この秒数が経過したら再びデモが始まる")]
    [SerializeField] protected float durationSeconds;

    /// <summary>
    /// チュートリアル画像がフェードインするときに発行されるイベント。
    /// </summary>
    protected fadeTutorial onFadeInTutorial;

    /// <summary>
    /// チュートリアル画像がフェードアウトするときに発行されるイベント。
    /// </summary>
    protected fadeTutorial onFadeOutTutorial;

    /// <summary>
    /// デモを始める。
    /// </summary>
    protected new void Start(){
        base.Start();

        mainBGMSource.Pause();

        //音楽をフェードするコルーチンを止める。
        //フェードのタイミングはTutorialCoroutineから指定する。
        StopCoroutine(fadeCoroutine);
        StartCoroutine(TutorialCoroutine());

        // デリゲートを空関数で初期化しておき、nullを防ぐ
        onFadeInTutorial = (int imageNum, float fadeTime) => { Debug.Log("Fade in"); };
        onFadeOutTutorial = (int  imageNum, float fadeTime) => { Debug.Log("Fade Out"); };
    }

    /// <summary>
    /// チュートリアルを行う。
    /// その後、フェードやデモの再読み込みのコルーチンを開始する。
    /// </summary>
    protected IEnumerator TutorialCoroutine()
    {
        onFadeOutTutorial(0, 5);

        mainBGMSource.Play();
        StartCoroutine(FadeCoroutine());
        StartCoroutine(RestartCoroutine());

        yield return null;
    }

    /// <summary>
    /// デモが終わるまで待って、すこし経つとまたデモを始める。
    /// </summary>
    protected IEnumerator RestartCoroutine()
    {
        yield return new WaitForSeconds(playSeconds + fadeTime * 2);
        onFadeInTutorial(0, durationSeconds);
        yield return new WaitForSeconds(durationSeconds);

        SceneManager.LoadScene("DemoScene");
    }

    /// <summary>
    /// フェードインのデリゲートに関数を登録する。
    /// </summary>
    public void RegisterOnFadeInTutorial(fadeTutorial e)
    {       
        onFadeInTutorial += e;
    }

    /// <summary>
    /// フェードアウトのデリゲートに関数を登録する。
    /// </summary>
    public void RegisterOnFadeOutTutorial(fadeTutorial e)
    {       
        onFadeOutTutorial += e;
    }
}
