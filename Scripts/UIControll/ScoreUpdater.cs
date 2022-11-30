using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// 拍動イベントを受けてUIを更新するコンポーネント
/// </summary>
public class ScoreUpdater : MonoBehaviour
{
    /// <summary>
    /// スコアを表示するUnityのテキスト。
    /// </summary>
    [SerializeField] Text scoreText;

    /// <summary>
    /// スコア。正しいタイミングで腕を振ると加算されていく。
    /// </summary>
    [SerializeField] int score;

    /// <summary>
    /// 現在続いているコンボ数。正しいタイミングで腕を振ると1ずつ増えていく。
    /// タイミングを外すと0に戻る。
    /// </summary>
    [SerializeField] int combo;

    /// <summary>
    /// コンボを表示するUnityのテキスト。
    /// </summary>
    [SerializeField] Text comboText;

    /// <summary>
    /// 正しいタイミングで腕を振ったときに獲得する基本スコア。
    /// </summary>
    const int BASE_SCORE = 10;

    /// <summary>
    /// コンボによるスコア倍率のカウンターストップ。
    /// コンボ数がCOMBO_STOPを超えた場合、それ以上倍率は増えない。
    /// </summary>
    const int COMBO_STOP = 20;

    /// <summary>
    /// scoreUpdaterのシングルトン。
    /// scoreUpdaterと名の付くオブジェクトがひとつしか存在しないことを保証する。
    /// </summary>
    public static ScoreUpdater scoreUpdater;
    
    private void Awake()
    {
        if(scoreUpdater == null)
        {
            scoreUpdater = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        var m = FindObjectOfType<MusicPase>();
        if(m != null)
        {
            m.RegisterOnJustTiming(this.OnJustTiming);
            m.RegisterOnOutOfTiming(this.OnOutOfTiming);
        }
    }

    /// <summary>
    /// スコアが変動する際の関数
    /// </summary>
    void UpdateUI()
    {
        scoreText.text = $"{score}";

        string combo_text = "";
        string comboMultiplier = getComboMultiplier().ToString("F1");
        //コンボ倍率を取得し、小数点以下1桁目までを表示する
        //（xx.xの形になる）

        comboText.text = 0 < combo ? $"{combo} COMBO\n×{comboMultiplier}" : "";
    }

    /// <summary>
    /// 今のコンボ数を元に、コンボによるスコア倍率を返す。
    /// </summary>
    float getComboMultiplier(){
        return  combo <= COMBO_STOP ? (1 + combo * 0.1f) : 3.0f;
    }

    /// <summary>
    /// MusicPaseで正しいリズムが刻まれた際に呼ばれるイベント
    /// </summary>
    void OnJustTiming()
    {
        score += (int)(10 * getComboMultiplier());
        combo++;
        UpdateUI();
    }

    /// <summary>
    /// MusicPaseで間違ったリズムが刻まれた際に呼ばれるイベント
    /// </summary>
    void OnOutOfTiming()
    {
        combo = 0;
        UpdateUI();
    }

    /// <summary>
    /// スコアをリセットする。
    /// </summary>
    void ResetScore()
    {
        score = 0;
        combo = 0; 
    }

    /// <summary>
    /// Resultシーン以外が読み込まれた場合、スコアをリセットする。
    /// </summary>
    void OnSceneLoaded( Scene scene, LoadSceneMode mode )
    {
        if(scene.name != "ResultScene") ResetScore();    
        
        scoreText = GameObject.Find("Score").GetComponent<Text>();
        comboText = GameObject.Find("Combo").GetComponent<Text>();
        UpdateUI();

        Debug.LogWarning($"Score: {score}");
    }
}
