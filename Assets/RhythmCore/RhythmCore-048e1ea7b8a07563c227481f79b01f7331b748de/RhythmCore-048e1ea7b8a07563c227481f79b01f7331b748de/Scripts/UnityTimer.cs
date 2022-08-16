using UnityEngine;
/// <summary>
/// UnityのTimeクラスを使用して実時間を返すクラス
/// </summary>
public class UnityTimer : ITimer
{
    float ITimer.GetCurrentTime()
    {
        return Time.realtimeSinceStartup;
    }
}
