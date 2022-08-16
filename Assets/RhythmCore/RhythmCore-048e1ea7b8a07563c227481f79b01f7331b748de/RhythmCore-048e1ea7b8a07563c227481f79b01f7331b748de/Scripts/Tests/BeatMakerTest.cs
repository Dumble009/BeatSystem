using NUnit.Framework;
using UnityEngine.TestTools.Utils;
using UnityEngine.TestTools;

/// <summary>
/// BeatMakerクラスのテストコード
/// </summary>
public class BeatMakerTest
{
    /// <summary>
    /// 登録されたイベントが呼び出された回数
    /// </summary>
    int eventCallCount;

    /// <summary>
    /// テスト対象のBeatMakerオブジェクト
    /// </summary>
    BeatMaker beatMaker;

    /// <summary>
    /// テスト変数の初期化を行う
    /// </summary>
    [SetUp]
    public void Init()
    {
        eventCallCount = 0;
        beatMaker = new BeatMaker(new MockTimer());
    }

    /// <summary>
    /// BeatMakerにイベントを登録することが出来、正しく呼ばれるかを確認するテスト
    /// </summary>
    [Test]
    public void RegisterEventTest()
    {
        // beatMakerにイベントを登録
        beatMaker.RegisterOnBeat(EventHandler);

        // 1回目の拍動の発生
        beatMaker.Beat();

        // 1回目の拍動ではまだイベントが呼ばれていないはず
        Assert.That(eventCallCount == 0);

        // 2回目の拍動の発生
        beatMaker.Beat();

        // 2回目の拍動で1回目のイベントコールが起こるはず
        Assert.That(eventCallCount == 1);
    }

    /// <summary>
    /// BeatMakerからイベントを登録解除できることを確認するテスト
    /// </summary>
    [Test]
    public void UnregisterEventTest()
    {
        // beatMakerにイベントを登録
        beatMaker.RegisterOnBeat(EventHandler);

        // イベントを登録解除
        beatMaker.UnregisterOnBeat(EventHandler);

        // 2回拍動が発生
        beatMaker.Beat();
        beatMaker.Beat();

        // イベントが登録解除されているので一回もイベントは呼ばれていないはず
        Assert.That(eventCallCount == 0);

    }

    /// <summary>
    /// OnBeatに登録されるイベント
    /// </summary>
    /// <param name="packet">イベントの引数として渡されるパケット</param>
    private void EventHandler(BeatPacket packet)
    {
        eventCallCount++;
    }
}
