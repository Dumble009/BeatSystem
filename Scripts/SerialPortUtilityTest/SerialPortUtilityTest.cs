using System.Collections;
using UnityEngine;

public class SerialPortUtilityTest : MonoBehaviour
{
    SerialPortUtility.SerialPortUtilityPro serialPort;
    bool isOpened = false;

    private void Awake() {
        serialPort = GetComponent<SerialPortUtility.SerialPortUtilityPro>();
    }

    IEnumerator Start(){
        yield return new WaitUntil(() => isOpened);
        Debug.Log("opened");

        LEDOff();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Return)){
            LEDOn();
        }else if(Input.GetKeyDown(KeyCode.Backspace)){
            LEDOff();
        }
    }

    private void LEDOff(){
        Debug.Log("write result : " + serialPort.WriteLF("0"));
    }

    private void LEDOn(){
        Debug.Log("write result : " + serialPort.WriteLF("1"));
    }

    public void Read(object o){
        string str = o as string;
        Debug.Log($"from arduino : {o as string}");

        if(str == "opened"){
            isOpened = true;
        }
    }
}
