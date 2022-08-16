using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 拍動イベントを受けてUIを更新するコンポーネント
/// </summary>
public class UIUpdater : MonoBehaviour
{
    /// <summary>
    /// 拍動回数のテキスト
    /// </summary>
    [SerializeField] Text beatCountText;

    /// <summary>
    /// 現在のテンポのテキスト
    /// </summary>
    [SerializeField] Text tempoText;

    /// <summary>
    /// テンポが正しく刻まれた回数を表すテキスト
    /// </summary>
    [SerializeField] Text justTimingCountText;

    /// <summary>
    /// テンポが正しく刻まれた回数
    /// </summary>
    [SerializeField] int justTimingCount;

    /// <summary>
    /// 拍動イベントを管理するオブジェクト
    /// </summary>
    [SerializeField] BeatMakerHolder beatMakerHolder;

    /// <summary>
    /// 正しいテンポの発生を管理するオブジェクト
    /// </summary>
    [SerializeField] MusicPase musicPase;

    private void Start()
    {
        beatMakerHolder.RegisterOnBeat(OnBeat);

        musicPase = FindObjectOfType<MusicPase>();
        musicPase.RegisterOnJustTiming(OnJustTiming);
    }

    /// <summary>
    /// 拍動発生時に呼ばれるイベント
    /// </summary>
    /// <param name="packet">拍動に関する情報</param>
    void OnBeat(BeatPacket packet)
    {
        beatCountText.text = $"BeatCount : {packet.BeatCount}";
        tempoText.text = $"Tempo : {packet.Tempo}";
    }

    /// <summary>
    /// 拍動発生時に呼ばれるイベント
    /// </summary>
    void OnJustTiming()
    {
        justTimingCountText.text = $"{++justTimingCount}";
    }
}
