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
    }
}
