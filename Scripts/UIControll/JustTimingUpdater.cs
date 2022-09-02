using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 拍動イベントを受けてUIを更新するコンポーネント
/// </summary>
public class JustTimingUpdater : MonoBehaviour
{
    /// <summary>
    /// テンポが正しく刻まれた回数を示すテキスト
    /// </summary>
    [SerializeField] Text justTimingText;

    /// <summary>
    /// テンポが正しく刻まれた回数
    /// </summary>
    [SerializeField] int justTimingCount;

    /// <summary>
    /// テンポが正しく刻まれた回数
    /// </summary>
    [SerializeField] Text RankText;

    private void Start()
    {
        var m = FindObjectOfType<MusicPase>();
        m.RegisterOnJustTiming(this.OnJustTiming);
    }

    /// <summary>
    /// MusicPaseで正しいイベントが刻まれた際に呼ばれるイベント
    /// </summary>
    void OnJustTiming()
    {
        justTimingText.text = $"{++justTimingCount}";

        string rank = "";
        if(1 <= justTimingCount) rank = "Good!";
        if(5 <= justTimingCount) rank = "Great!";
        if(10 <= justTimingCount) rank = "Super!";
        if(20 <= justTimingCount) rank = "Excellent!";
        if(30 <= justTimingCount) rank = "Perfect!";
        if(40 <= justTimingCount) rank = "Master!";
        if(50 <= justTimingCount) rank = "Are you kidding!?!?";
        RankText.text = rank;
    }
}
