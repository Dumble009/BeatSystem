/// <summary>
/// 拍動時のイベントの引数として渡されるパケット
/// </summary>
public class BeatPacket
{
    float tempo;
    /// <summary>
    /// 拍動と拍動の間の秒数
    /// </summary>
    public float Tempo
    {
        get
        {
            return tempo;
        }
        set
        {
            tempo = value;
        }
    }

    int beatCount;
    /// <summary>
    /// 何回目の拍動か
    /// </summary>
    public int BeatCount
    {
        get
        {
            return beatCount;
        }
        set
        {
            beatCount = value;
        }
    }
}
