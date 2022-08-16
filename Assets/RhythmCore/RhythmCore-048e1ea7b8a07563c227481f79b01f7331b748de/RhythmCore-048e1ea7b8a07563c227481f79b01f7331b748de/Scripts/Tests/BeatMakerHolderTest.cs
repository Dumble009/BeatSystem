using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools.Utils;
using UnityEngine.TestTools;
using UnityEngine;

/// <summary>
/// BeatMakerHolderクラスのテスト
/// </summary>
public class BeatMakerHolderTest
{
    /// <summary>
    /// 拍動回数のカウント
    /// </summary>
    int beatCount = 0;

    /// <summary>
    /// 拍動が発生した際に正しくイベントを実行することが出来るかのテスト
    /// </summary>
    [UnityTest]
    public IEnumerator BeatTest()
    {
        // GameObjectを作成して、コンポーネントをアタッチ
        var go = new GameObject();
        var holder = go.AddComponent<BeatMakerHolder>();

        holder.Maker = new MockBeatMaker(new MockTimer()); // BeatMakerHolderにモックを渡す


        // 拍動発生時のイベントを登録
        holder.RegisterOnBeat(EventHandler);

        // ホルダーに拍動の発生を通知
        holder.Beat();

        // 1回目の拍動ではイベントは呼ばれないはず
        Assert.That(beatCount == 0);

        // ホルダーに拍動の発生を通知
        holder.Beat();

        // 2回目の拍動で1回目のイベント呼び出しがあるはず
        Assert.That(beatCount == 1);

        // イベントの登録を解除する
        holder.UnregisterOnBeat(EventHandler);

        // ホルダーに拍動の発生を通知
        holder.Beat();

        // イベントの登録が解除されているのでbeatCountは変化しない
        Assert.That(beatCount == 1);

        yield return null;
    }

    /// <summary>
    /// OnBeatに登録されるイベント
    /// </summary>
    /// <param name="packet">イベントの引数として渡されるパケット</param>
    private void EventHandler(BeatPacket packet)
    {
        beatCount++;
    }
}
