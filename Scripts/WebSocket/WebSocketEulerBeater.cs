using UnityEngine;

/// <summary>
/// テンポが変化した時に発行するイベント
/// </summary>
/// <param name="angleY">針の角度。最小は0。最大は1</param>
public delegate void AngleYChangeHandler(float needleValue);

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
    /// スマホの角度の変化時に発行するイベント
    /// </summary>
    AngleYChangeHandler onAngleYChange;

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

            float needleValue = GetNeedleValue(angleY);
            onAngleYChange(needleValue);
        }
    }
 
    /// <summary>
    /// 角度が変化した際のイベントにメッセージを登録する。
    /// </summary>
    /// <param name="e">角度が変化した際に呼び出される処理</param>
    public void RegisterOnAngleYChange(AngleYChangeHandler e)
    {
        onAngleYChange += e;
    }

     /// <summary>
    /// OnTempoChangeの引数で渡された現在のテンポを元に針が指示する値を決定する
    /// </summary>
    /// <param name="angleY">スマホの角度</param>
    private float GetNeedleValue(float angleY)
    {
        if(angleY > 90){
            angleY = 180 - angleY;
        }else if(angleY < -90){
            angleY = -180 - angleY;
        }


        // angleYがdownThresholdならNeedleValueは1
        // angleYがupThresholdならNeedleValueは0
        float needleValue = (angleY - upThreshold) / (downThreshold - upThreshold);

        Debug.Log($"angleY = {angleY}, needleValue = {needleValue}");

        if(needleValue < 0) needleValue = 0;
        if(1 < needleValue) needleValue = 1;



        return needleValue;
    }
}
