using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// WebSocketEulerBeaterからのイベントを購読して針を動かす
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
        eulerBeater.RegisterOnAngleChange(this.onAngleChange); 
    }

    /// <summary>
    /// スマホの角度が変化したときに呼び出される関数
    /// </summary>
    /// <param name="angle">現在のスマホの角度が送られる。downThresholdなら0。upThresholdなら1</param>
    private void onAngleChange(float angle)
    {
        foreach (var n in needle)
        {
            n.SetValue(angle);
        }
    }
}
