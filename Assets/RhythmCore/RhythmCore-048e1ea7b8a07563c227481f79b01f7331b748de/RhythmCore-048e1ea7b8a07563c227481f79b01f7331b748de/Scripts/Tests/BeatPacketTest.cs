using NUnit.Framework;
using UnityEngine.TestTools.Utils;

/// <summary>
/// BeatPacketクラスのテストコード
/// </summary>
public class BeatPacketTest
{
    /// <summary>
    /// float値を比較する際に許容できる相対誤差の上限
    /// </summary>
    const float allowedRelativeError = 10e-6f;

    /// <summary>
    /// BeatCountを正しく保持できるかのテスト
    /// </summary>
    [Test]
    public void BeatCountTest()
    {
        var packet = new BeatPacket();

        // BeatCountの初期値は0
        Assert.That(packet.BeatCount == 0);

        packet.BeatCount++;

        // BeatCountが正しく加算される
        Assert.That(packet.BeatCount == 1);

        // BeatCountのインクリメントを100回行う
        for (int i = 0; i < 100; i++)
        {
            packet.BeatCount++;
        }

        // BeatCountが正しく加算されることを確認
        Assert.That(packet.BeatCount == 101);
    }

    /// <summary>
    /// Tempoを正しく保持できるかのテスト
    /// </summary>
    [Test]
    public void TempoTest()
    {
        var packet = new BeatPacket();

        // Tempoの初期値は0.0
        Assert.That(Utils.AreFloatsEqual(0.0f, packet.Tempo, allowedRelativeError));

        packet.Tempo = 1.0f;

        // Tempoが上書きできる
        Assert.That(Utils.AreFloatsEqual(1.0f, packet.Tempo, allowedRelativeError));

        // Tempoの増加を100回行う
        for (int i = 0; i < 100; i++)
        {
            packet.Tempo += 1.0f;
        }

        // Tempoの加算が正常に出来る
        Assert.That(Utils.AreFloatsEqual(101.0f, packet.Tempo, allowedRelativeError));

    }
}
