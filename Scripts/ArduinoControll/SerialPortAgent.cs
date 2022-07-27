using UnityEngine;

/// <summary>
/// NeedleControllerから渡された値をSerialPortUtilityProを通してArduinoに送る
/// </summary>
public class SerialPortAgent : MonoBehaviour, INeedle
{
    SerialPortUtility.SerialPortUtilityPro serialPort;

    private void Awake()
    {
        serialPort = GetComponent<SerialPortUtility.SerialPortUtilityPro>();
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
        }
    }
}