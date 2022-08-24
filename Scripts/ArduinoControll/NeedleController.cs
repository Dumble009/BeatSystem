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
        var eulerBeater = FindObjectOfType<WebSocketEulerBeater>();
        eulerBeater.RegisterOnAngleYChange(this.OnAngleYChange);
    }

    /// <summary>
    /// スマホの角度が変化した時に呼び出される関数
    /// </summary>
    /// <param name="angleY">スマホの角度。真上は90。真下は-90</param>
    private void OnAngleYChange(float angleY)
    {
        foreach (var n in needle)
        {
            n.SetValue(angleY);
        }
    }
}
