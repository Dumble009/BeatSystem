using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic; 

/// <summary>
/// 拍動イベントを受けてUIを更新するコンポーネント
/// </summary>
public class TutorialUpdater : MonoBehaviour
{
    /// <summary>
    /// チュートリアルを表示するための白い背景。
    /// </summary>
    [SerializeField] protected List<CanvasGroup> tutorial;


    private void Start()
    {
        var demo = FindObjectOfType<DemoSceneController>();

        Debug.Log(demo);
        if(demo != null)
        {
            demo.RegisterOnFadeInTutorial(this.OnFadeInTutorial);
            demo.RegisterOnFadeOutTutorial(this.OnFadeOutTutorial);
        }
    }

    /// <summary>
    /// チュートリアル画像のフェードイン・フェードアウトを行うコルーチンを起動させる。
    /// 画像は番号で選択する。
    /// </summary>
    /// <param name="imageNum">チュートリアル画像の番号。0はカバー</param>
    /// <param name="fadeTime">画像が透明度0から1になるまでの時間</param>
    protected void OnFadeInTutorial(int imageNum, float fadeTime)
    {
        int num = Mathf.Clamp(imageNum, 0, tutorial.Count - 1);
        StartCoroutine(FadeInTutorial( tutorial[num], fadeTime ));
    }

    /// <summary>
    /// 画像の透明度を徐々に徐々に下げる。
    /// </summary>
    /// <param name="image">チュートリアル画像を含むキャンバスグループ</param>
    /// <param name="fadeTime">画像が透明度0から1になるまでの時間</param>
    protected IEnumerator FadeInTutorial(CanvasGroup image, float fadeTime)
    {
        //imageの透明度を時間経過で0に近付ける
        for (float i = 0.0f; i <= 1.0f; i += Time.deltaTime / fadeTime){
            image.alpha = i;
        }

        //Debug.Log($"{image}, {image.alpha}");
        yield return null;
    }

    /// <summary>
    /// チュートリアル画像のフェードイン・フェードアウトを行うコルーチンを起動させる。
    /// 画像は番号で選択する。
    /// </summary>
    /// <param name="imageNum">チュートリアル画像の番号。0はカバー</param>
    /// <param name="fadeTime">画像が透明度が1から0になるまでの時間</param>
    protected void OnFadeOutTutorial(int imageNum, float fadeTime)
    {
        int num = Mathf.Clamp(imageNum, 0, tutorial.Count - 1);
        StartCoroutine(FadeOutTutorial( tutorial[num], fadeTime ));
    }

    /// <summary>
    /// 画像の透明度を徐々に徐々に上げる。
    /// </summary>
    /// <param name="image">チュートリアル画像を含むキャンバスグループ</param>
    /// <param name="fadeTime">画像が透明度1から0になるまでの時間</param>
    protected IEnumerator FadeOutTutorial(CanvasGroup image, float fadeTime)
    {
        //imageの透明度を時間経過で0に近付ける
        for (float i = 1.0f; i >= 0.0f; i += Time.deltaTime / fadeTime){
            //image.alpha = i;
        }

        Debug.Log($"{image}, {image.alpha}");
        yield return null;
    }
}