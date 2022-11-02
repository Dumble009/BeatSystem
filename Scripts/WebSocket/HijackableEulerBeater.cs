using UnityEngine;

/// <summary>
/// WebSocket経由で送信されてきた端末のオイラー角を使用して端末の姿勢を推定・拍動の検出を行う。
/// </summary>
public class HijackableEulerBeater : WebSocketEulerBeater
{
    /// <summary>
    /// 最後にスマホが人に持たれてからの秒数
    /// </summary>
    protected float timeSinceIPhoneMoved = 5.0f;

    /// <summary>
    /// 音楽のテンポ。
    /// 単位は拍と拍の感覚の秒数。
    /// </summary>
    protected float originalTempo = 1;

    /// <summary>
    /// 初期化する。
    /// </summary>
    protected void Start()
    {
        base.Start();

        var m = FindObjectOfType<MusicPase>();
        originalTempo = m.OriginalTempo;
    }

    /// <summary>
    /// WebSocketからメッセージを受信した際に呼ばれるコールバック関数
    /// </summary>
    /// <param name="msg">受信文字列</param>
    protected new void OnMsg(string msg)
    {
        // オイラー角はコロン区切りで送られてくる
        string[] vals = msg.Split(':');

        // ちょうど3つに区切れなければ不正な値が返ってきている
        if (vals.Length != 3)
        {
            return;
        }

        // メッセージから針の角度を取得する
        float angleY = float.Parse(vals[1]);

        //スマホの角度によってスマホが動いているかどうか判断する。
        //動いていなければ、勝手に針を動かす。
        angleY = isIPhoneMoving(angleY) ? angleY : CalcFalseAngleY();
        MoveNeedle(angleY);
    }

    /// <summary>
    /// スマホの角度によって、スマホが動いているかどうかを判断する。
    /// </summary>
    /// <param name="msg">受信文字列</param>
    protected bool isHijackingNeedle(float angleY)
    {
        //スマホの角度が上か下に20°より大きければ
        //「最後にスマホが人に持たれてからの時間」を0にする
        if( 20 <= Mathf.Abs(angleY) )
        {
            timeSinceIPhoneMoved = 0;
        }

        //最後にスマホが人に持たれてからの時間を、時間経過で増やす
        timeSinceIPhoneMoved += Time.deltaTime;

        //スマホが最後に動いてから5秒以内はスマホが動いていると判定
        return timeSinceIPhoneMoved < 5;
    }

    /// <summary>
    /// 経過時間を元にスマホの角度を捏造する。
    /// ここで得られた角度をMoveAngleに渡すと針が自動で動く。
    /// </summary>
    protected float CalcFalseAngleY()
    {
        float amplitude = upThreshold - downThreshold;
        float falseAngleY = (upThreshold + downThreshold) / 2;
        falseAngleY += amplitude * Mathf.Sin(Time.time / originalTempo * 2 * Mathf.PI);

        return falseAngleY;
    }
}
