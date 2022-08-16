using UnityEngine;

/// <summary>
/// スマホの角度が変化した時に発行するイベント
/// </summary>
/// <param name="angle">現在のスマホの角度が送られる。downThresholdなら0。upThresholdなら1</param>
public delegate void AngleChangeHandler(float angle);

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
    [Header("これ以上上げたらダンベルを持ち上げたと判定する角度(デグリー)")]
    [SerializeField] float upThreshold;

    /// <summary>
    /// これ以上下げたらダンベルを下げたと判定する角度(デグリー)。-90度で真下。
    /// </summary>
    [Header("これ以上下げたらダンベルを下げたと判定する角度(デグリー)")]
    [SerializeField] float downThreshold;

    /// <summary>
    /// 今ダンベルを持ち上げようとしているかどうか
    /// </summary>
    bool isRising = true;

    /// <summary>
    /// スマホの角度変化時に発生するイベント
    /// </summary>
    AngleChangeHandler onAngleChange;

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
            
            //角度が（まず間違いなく変化しているので）変化した事をきっかけに色んな関数を起動
            float angle = getAngleValue(angleY);
            onAngleChange(angle);

            //テンポが変化したら（一回分の店舗が数えられたら）それをきっかけに色んな関数を起動
            if (isRising && angleY >= upThreshold ||
                !isRising && angleY <= downThreshold)
            {
                isRising = !isRising;
                holder.Beat();
            }
        }
    }

    /// <summary>
    /// スマホの角度が変化した際のイベントにメッセージを登録する。
    /// </summary>
    /// <param name="angleY">スマホの角度。鉛直上向きで90。鉛直下向きで-90</param>
    float getAngleValue(float angleY){
        if(angleY <= downThreshold) return 0;
        if(angleY >= upThreshold) return 1;
    
        //angleY = -90で0
        //angleY = 90で1
        return (angleY + 90) / (90 + 90);
    }

    /// <summary>
    /// スマホの角度が変化した際のイベントにメッセージを登録する。
    /// </summary>
    /// <param name="e">スマホの角度が変化した際に呼び出される処理</param>
    public void RegisterOnAngleChange(AngleChangeHandler e)
    {
        onAngleChange += e;
    }
}