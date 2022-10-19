using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// 拍動イベントを受けてUIを更新するコンポーネント
/// </summary>
public class ScoreUpdater : MonoBehaviour
{
    /// <summary>
    /// スコアを示すテキスト
    /// </summary>
    [SerializeField] Text scoreText;

    /// <summary>
    /// スコア
    /// </summary>
    [SerializeField] float score;

    /// <summary>
    /// コンボ
    /// </summary>
    [SerializeField] int combo;

    /// <summary>
    /// コンボに応じて変動する文章
    /// </summary>
    [SerializeField] Text comboText;

    private void Start()
    {
        var m = FindObjectOfType<MusicPase>();
        m.RegisterOnJustTiming(this.OnJustTiming);
        m.RegisterOnOutOfTiming(this.OnOutOfTiming);
    }


    /// <summary>
    /// スコアが変動する際の関数
    /// </summary>
    void UpdateUI()
    {
        scoreText.text = $"{Math.Round(score)}";

        string comboMultiplier = (1 + combo * 0.1f).ToString("F1");
        string combo_text = "";

        if(0 < combo) combo_text = $"{combo} combo\n×{comboMultiplier}";
        if(20 <= combo){
            combo_text = "MAX COMBO!!\n×3.0";

        }
        comboText.text = combo_text;
    }

    /// <summary>
    /// MusicPaseで正しいリズムが刻まれた際に呼ばれるイベント
    /// </summary>
    void OnJustTiming()
    {
        score += 10 * (1 + combo * 0.1f);
        UpdateUI();
        
        if(combo < 20) combo++;
    }

    /// <summary>
    /// MusicPaseで間違ったリズムが刻まれた際に呼ばれるイベント
    /// </summary>
    void OnOutOfTiming()
    {
        combo = 0;
        UpdateUI();
    }
}
