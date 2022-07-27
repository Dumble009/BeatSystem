using UnityEngine;

public class MessageSender : MonoBehaviour
{
    SerialPortUtility.SerialPortUtilityPro serialPort;

    private void Awake()
    {
        serialPort = GetComponent<SerialPortUtility.SerialPortUtilityPro>();
    }

    public void SendToArduino(float f)
    {
        if (f < 0)
        {
            f = 0;
        }else if (f > 1)
        {
            f = 1;
        }
        int val = (int)f * 180;
        if (serialPort != null)
        {
            if (serialPort.IsOpened())
            {
                serialPort.WriteLF(val.ToString());
            }
        }
    }
}