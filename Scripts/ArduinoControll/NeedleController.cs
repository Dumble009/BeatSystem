using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MusicPaseからのイベントを購読して
/// </summary>
public class NeedleController : MonoBehaviour
{
    /// <summary>
    /// 操作対象の針のリスト
    /// </summary>
    List<INeedle> needle;

    private void Awake()
    {
        needle = new List<INeedle>();
        var childrenNeedles = GetComponentsInChildren<INeedle>();
        foreach (var n in childrenNeedles)
        {
            needle.Add(n);
        }
    }

    private void Start()
    {
        var musicPase = FindObjectOfType<MusicPase>();
        musicPase.RegisterOnTempoChange(this.OnTempoChange);
    }

    /// <summary>
    /// 音楽のテンポが変化した時に呼び出される関数
    /// </summary>
    /// <param name="normalizedTempo">現在の音楽のテンポデフォルトのテンポが1、それより倍なら2、半分なら0.5が送られる</param>
    private void OnTempoChange(float normalizedTempo)
    {
        float needleValue = GetNeedleValue(normalizedTempo);
        foreach (var n in needle)
        {
            n.SetValue(needleValue);
        }
    }

    /// <summary>
    /// OnTempoChangeの引数で渡された現在のテンポを元に針が指示する値を決定する
    /// </summary>
    /// <param name="normalizedTempo">現在のテンポ</param>
    private float GetNeedleValue(float normalizedTempo)
    {
        // 現在のテンポが1なら0.5(真ん中)を指し、2倍以上なら大きい方に振り切る(1)、小さい方にはテンポが完全に0にならない限り振り切らない
        if (normalizedTempo >= 2.0f)
        {
            return 1.0f;
        }

        return normalizedTempo / 2.0f;
    }
}
