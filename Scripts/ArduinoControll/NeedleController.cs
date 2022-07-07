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
    private void OnTempoChange(float normalizedTempo)
    {

    }
}
