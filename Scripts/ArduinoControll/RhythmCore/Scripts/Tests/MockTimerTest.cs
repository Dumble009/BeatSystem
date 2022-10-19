using NUnit.Framework;
using UnityEngine.TestTools.Utils;

/// <summary>
/// MockTimerクラスのテスト
/// </summary>
public class MockTimerTest
{
    /// <summary>
    /// float値を比較する際に許容できる相対誤差の上限
    /// </summary>
    const float allowedRelativeError = 10e-6f;

    /// <summary>
    /// MockTimerが正しい時刻を返すかどうかのテスト
    /// </summary>
    [Test]
    public void TimeCheckTest()
    {
        MockTimer timer = new MockTimer();

        // 現在時刻を0秒に設定
        timer.CurrentTime = 0.0f;

        // 現在時刻が0秒か確認
        Assert.That(Utils.AreFloatsEqual(0.0f, timer.GetCurrentTime(), allowedRelativeError));

        // 現在時刻を0.5秒に設定
        timer.CurrentTime = 0.5f;

        // 現在時刻が0秒か確認
        Assert.That(Utils.AreFloatsEqual(0.5f, timer.GetCurrentTime(), allowedRelativeError));

        // 現在時刻を100.0秒に設定
        timer.CurrentTime = 100.0f;

        // 現在時刻が0秒か確認
        Assert.That(Utils.AreFloatsEqual(100.0f, timer.GetCurrentTime(), allowedRelativeError));
    }
}
