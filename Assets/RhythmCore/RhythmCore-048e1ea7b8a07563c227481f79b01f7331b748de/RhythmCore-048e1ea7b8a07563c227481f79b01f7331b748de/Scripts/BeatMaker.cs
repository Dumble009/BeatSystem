
/// <summary>
/// 拍動イベント用のデリゲート
/// </summary>
/// <param name="packet">今回の拍動に関するパケット</param>
public delegate void BeatHandler(BeatPacket packet);

/// <summary>
/// 拍動を元にテンポの計算、イベントの発行を行うクラス
/// </summary>
public class BeatMaker
{
    /// <summary>
    /// 拍動時のイベントハンドラ
    /// </summary>
    BeatHandler beatHandler;

    /// <summary>
    /// 実時間を取得するために使用されるタイマー
    /// </summary>
    protected ITimer timer;

    /// <summary>
    /// 拍動の回数。1-indexed
    /// </summary>
    int beatCount;
    public BeatMaker(ITimer _timer)
    {
        // 空関数で初期化することでnullを回避する
        beatHandler = (x) => { };

        beatCount = 0;

        timer = _timer;
    }

    /// <summary>
    /// 外部のオブジェクトが拍動事象発生時に呼び出す関数
    /// </summary>
    public void Beat()
    {
        // BeatPacketオブジェクトに拍動に関する情報を詰め込んでイベントに渡す
        var packet = new BeatPacket();
        beatCount++;
        packet.BeatCount = beatCount;
        packet.Tempo = CalcTempo();

        // 1回目の拍動からはテンポが計算できないのでイベントの発行は2回目の拍動から始める
        if (beatCount > 1)
        {
            beatHandler(packet);
        }
    }

    /// <summary>
    /// 拍動時のイベントの登録
    /// </summary>
    /// <param name="handler">登録されるイベント</param>
    public void RegisterOnBeat(BeatHandler handler)
    {
        beatHandler += handler;
    }

    /// <summary>
    /// 拍動時のイベントの登録解除
    /// </summary>
    /// <param name="handler">登録解除されるイベント</param>
    public void UnregisterOnBeat(BeatHandler handler)
    {
        beatHandler -= handler;
    }

    /// <summary>
    /// テンポ(拍動間の秒数)の計算。計算にどのような情報が必要になるか分からないので、必要な情報は引数ではなくクラスのメンバ変数として持つようにする。
    /// </summary>
    /// <returns></returns>
    virtual protected float CalcTempo()
    {
        return 0.0f;
    }
}