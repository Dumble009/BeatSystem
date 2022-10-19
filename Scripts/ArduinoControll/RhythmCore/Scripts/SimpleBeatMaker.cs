using UnityEngine;

/// <summary>
/// 前の拍からの経過秒数をそのままテンポとするBeatMaker
/// </summary>
public class SimpleBeatMaker : BeatMaker
{
    /// <summary>
    /// 実行開始時から最後の拍動が打たれた時までの経過秒数
    /// </summary>
    float lastBeatTime;
    public SimpleBeatMaker(ITimer _timer) : base(_timer)
    {
        lastBeatTime = 0.0f;
    }
    protected override float CalcTempo()
    {
        float currentTime = timer.GetCurrentTime();
        float tempo = currentTime - lastBeatTime;
        lastBeatTime = currentTime;

        return tempo;
    }
}
