using UnityEngine;

/// <summary>
/// スマホの姿勢を正しく取得するための調査スクリプト
/// </summary>
public class QuaternionExample : MonoBehaviour
{
    Vector4 lastRot;
    Vector3 upInCel;

    public void Receive(Vector4 rot)
    {
        lastRot = rot;
    }

    bool isCalibrating = false;

    private void Update()
    {
        var rot = new Quaternion(lastRot.x, lastRot.y, lastRot.z, lastRot.w);
        var currentUp = rot * Vector3.up;
        if (Input.GetKeyDown(KeyCode.Return))
        {
            upInCel = currentUp;
        }
        Debug.DrawLine(Vector3.zero, upInCel * 5.0f, Color.red);
        Debug.DrawLine(Vector3.zero, currentUp * 5.0f, Color.blue);

        Debug.Log(Vector3.Angle(upInCel, currentUp));
    }

}
