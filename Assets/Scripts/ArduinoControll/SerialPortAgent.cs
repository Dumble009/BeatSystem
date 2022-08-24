using System.Collections;
using UnityEngine;

/// <summary>
/// NeedleControllerから渡された値をSerialPortUtilityProを通してArduinoに送る
/// </summary>
public class SerialPortAgent : MonoBehaviour, INeedle
{
    SerialPortUtility.SerialPortUtilityPro serialPort;

    /// <summary>
    /// ArduinoがWriteを受け付けられる状態になっているかどうか
    /// </summary>
    bool isOpened = false;

    /// <summary>
    /// このメッセージがArduinoから送られてきたら送信可能と判定
    /// </summary>
    const string OPEN_MESSAGE = "opened";

    /// <summary>
    /// 針が振れる最低の角度
    /// </summary>
    [SerializeField] int minAngle = 30;
    /// <summary>
    /// 針が振れる最大の角度
    /// </summary>
    [SerializeField] int maxAngle = 150;

    private void Awake()
    {
        serialPort = GetComponent<SerialPortUtility.SerialPortUtilityPro>();
    }

    public void SetValue(float f)
    {
        f = 1 - f;
        if (f < 0)
        {
            f = 0;
        }else if (f > 1)
        {
            f = 1;
        }
        int val = minAngle + (int)(f * (maxAngle - minAngle));
        if (serialPort != null)
        {
            if (isOpened)
            {
                //Debug.Log($"send, {val} : {f}");
                serialPort.WriteLF(val.ToString());
            }else{
                Debug.Log("Arduino Not Opened");
            }
        }
    }

    private void OnDestroy() {
        serialPort.Close();
    }

    /// <summary>
    /// Arduinoからシリアル通信でメッセージが送られてきたときに呼び出される。メッセージは文字列だが、obj型に格納されているのでキャストする必要がある。
    /// </summary>
    /// <param name="obj">メッセージオブジェクト</param>
    public void Receive(object obj){
        string message = obj as string;
        //Debug.Log($"send from arduino {message}");

        // 送られてきたメッセージがArduino起動時のOpenメッセージだった場合は送信可能フラグを立てる
        if(message == OPEN_MESSAGE){
            Debug.Log("port opened");
            isOpened = true;
        }
    }
}