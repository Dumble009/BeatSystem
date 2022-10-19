
/// <summary>
/// テスト用のITimerオブジェクトのモック
/// </summary>
internal class MockTimer : ITimer
{
    float currentTime;

    /// <summary>
    /// 現在時間
    /// </summary>
    public float CurrentTime
    {
        get
        {
            return currentTime;
        }
        set
        {
            currentTime = value;
        }
    }

    public MockTimer()
    {
        currentTime = 0.0f;
    }

    /// <summary>
    /// 設定されたcurrentTimeをそのまま返す
    /// </summary>
    /// <returns>ダミーの現在時間</returns>
    public float GetCurrentTime()
    {
        return currentTime;
    }
}