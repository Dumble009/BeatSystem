using System.Collections;
using UnityEngine;

/// <summary>
/// NeedleControllerから渡された値をSerialPortUtilityProを通してArduinoに送る
/// </summary>
public class SerialPortAgent : MonoBehaviour, INeedle
{
    /// <summary>
    /// 最後にArduinoに情報を送った時間
    /// </summary>
    float lastSentTime = 0.0f;
    SerialPortUtility.SerialPortUtilityPro serialPort;

    private void Awake()
    {
        lastSentTime = Time.realtimeSinceStartup;
        serialPort = GetComponent<SerialPortUtility.SerialPortUtilityPro>();
    }

    private IEnumerator Start() {
        yield return new WaitUntil(() => serialPort.IsOpened());
        float val = 0.0f;
        while (true)
        {
            yield return new WaitForSeconds(1f);
            val += 10.0f;
            //yield return new WaitForSeconds(1.0f);
            Debug.Log($"start! : {serialPort.IsOpened()}");
            serialPort.WriteLF(val.ToString());
        }
    }

    public void SetValue(float f)
    {
        if (f < 0)
        {
            f = 0;
        }else if (f > 1)
        {
            f = 1;
        }
        int val = (int)(f * 180);
        if (serialPort != null)
        {
            if (serialPort.IsOpened())
            {
                Debug.Log($"send, {val} : {f}");
                serialPort.WriteLF(val.ToString());
            }
            lastSentTime = Time.realtimeSinceStartup;
        }
    }

    private void OnDestroy() {
        serialPort.Close();
    }
}