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
    /// この回数デモの中でポイントが入ったら本番が始まる。
    /// </summary>
    [Header("この回数デモの中でポイントが入ったら本番が始まる。")]
    [SerializeField] protected int untilMainPhase;
    protected int initialUntilMainPhase;

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

    [SerializeField] protected HijackableEulerBeater hijackableEulerBeater;

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

        initialUntilMainPhase = untilMainPhase;

        var m = FindObjectOfType<MusicPase>();
        if (m != null)
        {
            m.RegisterOnJustTiming(this.OnGetScore);
        }
        else
        {
            Debug.LogError("There isn't MusicPase Component.");
        }

        mainBGMSource.Pause();

        // デリゲートを空関数で初期化しておき、nullを防ぐ
        onFadeInTutorial = (int imageNum, float fadeTime) => { Debug.Log("Fade in"); };
        onFadeOutTutorial = (int  imageNum, float fadeTime) => { Debug.Log("Fade Out"); };

        //音楽をフェードするコルーチンを止める。
        //フェードのタイミングはTutorialCoroutineから指定する。
        StopCoroutine(fadeCoroutine);
        StartCoroutine(TutorialCoroutine());
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
        StartCoroutine(StartMainPhaseCoroutine());

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
    /// 5回振ったらデモが終わる。
    /// </summary>
    protected IEnumerator StartMainPhaseCoroutine()
    {
        while(untilMainPhase > 0 ){
            yield return null;
        }

        Debug.Log("Starting Main Phase");
        onFadeInTutorial(0, durationSeconds);
        //yield return new WaitForSeconds(durationSeconds);
        SceneManager.LoadScene("MusicScene");
    }

    /// <summary>
    /// ちょうどいいタイミングで腕を振れば、本番までの時間を短める。
    /// </summary>
    public void OnGetScore()
    {
        if(hijackableEulerBeater.isControlledByHuman){
            untilMainPhase--;
        }else{
            untilMainPhase = initialUntilMainPhase;
        }
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
