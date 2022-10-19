using NUnit.Framework;
using UnityEngine.TestTools.Utils;

/// <summary>
/// SimpleBeatMakerクラスのテストコード
/// </summary>
public class SimpleBeatMakerTest
{
    /// <summary>
    /// テスト対象のBeatMakerオブジェクト
    /// </summary>
    BeatMaker b;

    /// <summary>
    /// テスト対象に渡すITimerのモック
    /// </summary>
    MockTimer timer;

    /// <summary>
    /// 拍動発生時のイベントの引数
    /// </summary>
    BeatPacket packet;

    /// <summary>
    /// float値を比較する際に許容できる相対誤差の上限
    /// </summary>
    const float allowedRelativeError = 10e-6f;

    /// <summary>
    /// テスト変数の初期化を行う
    /// </summary>
    [SetUp]
    public void Init()
    {
        timer = new MockTimer();
        b = new SimpleBeatMaker(timer);

        b.RegisterOnBeat((p) => { packet = p; }); // 拍動イベントの発生時のイベント登録
    }

    /// <summary>
    /// 一定間隔(1秒ごと)での拍動発生時のテスト
    /// </summary>
    [Test]
    public void ConstantTempoTest1()
    {
        ConstantTempoTest(1.0f);
    }

    /// <summary>
    /// 一定間隔(100秒ごと)での拍動発生時のテスト
    /// </summary>
    [Test]
    public void ConstantTempoTest2()
    {
        ConstantTempoTest(100.0f);
    }

    /// <summary>
    /// 拍動間隔が一定でない場合のテスト
    /// </summary>
    [Test]
    public void NonConstantTempoTest1()
    {
        var packet = new BeatPacket(); // 拍動イベントの引数をコピーしておく入れ物

        // 時刻0秒で拍動が発生
        timer.CurrentTime = 0.0f;
        b.Beat();

        // 時刻1秒で拍動が発生
        timer.CurrentTime = 1.0f;
        b.Beat();

        // この時点で拍動の回数が2回、テンポが1.0のはず
        AssertBeatCountAndTempo(2, 1.0f);

        timer.CurrentTime = 3.0f;
        b.Beat();

        // この時点で拍動の回数が3回、テンポが2.0のはず
        AssertBeatCountAndTempo(3, 2.0f);

        // 時刻100秒で拍動が発生
        timer.CurrentTime = 100.0f;
        b.Beat();

        // この時点で拍動の回数が4回、テンポが97.0のはず
        AssertBeatCountAndTempo(4, 97.0f);
    }

    /// <summary>
    /// 拍動間隔が小数(0.5秒)の時のテスト
    /// </summary>
    [Test]
    public void ConstantFloatTest1()
    {
        ConstantTempoTest(0.5f);
    }

    /// <summary>
    /// 拍動間隔が小数(0.123456789秒)の時のテスト
    /// </summary>
    [Test]
    public void ConstantFloatTest2()
    {
        ConstantTempoTest(0.123456789f);
    }

    /// <summary>
    /// 一定間隔での拍動発生時のテストのテンプレート
    /// </summary>
    /// <param name="tempo">拍動間の秒数</param>
    private void ConstantTempoTest(float tempo)
    {

        // 時刻0秒に1回目の拍動が発生
        timer.CurrentTime = 0.0f;
        b.Beat();

        // 時刻1秒に2回目の拍動が発生
        timer.CurrentTime += tempo;
        b.Beat();

        // この時点でのpacketの情報が、拍動の回数が2回、予想テンポが1.0秒になっているはず
        AssertBeatCountAndTempo(2, tempo);

        // 時刻2秒に3回目の拍動が発生
        timer.CurrentTime += tempo;
        b.Beat();

        // この時点でのpacketの情報が、拍動の回数が3回、予想テンポが1.0秒になっているはず
        AssertBeatCountAndTempo(3, tempo);

        // その後時刻100秒まで毎秒拍動が発生
        for (int i = 3; i <= 100; i++)
        {
            timer.CurrentTime += tempo;
            b.Beat();
        }

        // この時点でのpacketの情報が、拍動の回数が101回、予想テンポが1.0秒になっているはず
        AssertBeatCountAndTempo(101, tempo);
    }

    /// <summary>
    /// packetオブジェクトの中の拍動回数とテンポの情報をテストする
    /// </summary>
    /// <param name="beatCount">正解の拍動回数</param>
    /// <param name="tempo">正解のテンポ</param>
    private void AssertBeatCountAndTempo(int beatCount, float tempo)
    {
        Assert.That(packet.BeatCount == beatCount); // 拍動回数のテスト
        Assert.That(Utils.AreFloatsEqual(tempo, packet.Tempo, allowedRelativeError)); // テンポのテスト
    }
}