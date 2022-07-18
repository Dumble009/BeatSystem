using UnityEngine;

/// <summary>
/// WebSocket経由で送信されてきた端末のオイラー角を使用して端末の姿勢を推定・拍動の検出を行う。
/// </summary>
public class WebSocketEulerBeater : MonoBehaviour
{
    [SerializeField] WebSocketReceiver receiver;
    [SerializeField] BeatMakerHolder holder;

    /// <summary>
    /// これ以上上げたらダンベルを持ち上げたと判定する角度(デグリー)。90度で真上
    /// </summary>
    [SerializeField] float upThreshold;

    /// <summary>
    /// これ以上下げたらダンベルを下げたと判定する角度(デグリー)。-90度で真下。
    /// </summary>
    [SerializeField] float downThreshold;

    /// <summary>
    /// 今ダンベルを持ち上げようとしているかどうか
    /// </summary>
    bool isRising = true;

    private void Start()
    {
        receiver.RegisterOnReceiveMessage(OnMsg);
    }

    /// <summary>
    /// WebSocketからメッセージを受信した際に呼ばれるコールバック関数
    /// </summary>
    /// <param name="msg">受信文字列</param>
    void OnMsg(string msg)
    {
        // オイラー角はコロン区切りで送られてくる
        string[] vals = msg.Split(':');

        // ちょうど3つに区切れなければ不正な値が返ってきている
        if (vals.Length == 3)
        {
            // 端末の左右がy軸に相当するのでy軸中心のオイラー角が端末が上を向いているか下を向いているかを示している
            float angleY = float.Parse(vals[1]);

            if (isRising && angleY >= upThreshold ||
                !isRising && angleY <= downThreshold)
            {
                isRising = !isRising;
                holder.Beat();
            }
        }
    }
}
