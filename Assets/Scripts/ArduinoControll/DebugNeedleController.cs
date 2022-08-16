using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Arduinoの通信を確認するためにUIを読み取ってINeedleに値を渡す
/// </summary>
public class DebugNeedleController : MonoBehaviour
{
    /// <summary>
    /// INeedleに送る値を書き込むUI
    /// </summary>
    [SerializeField] InputField inputValue;

    /// <summary>
    /// 操作対象のINeedleオブジェクト
    /// </summary>
    INeedle needle;

    private void Awake() {
        needle = GetComponentInChildren<INeedle>();
    }
    
    /// <summary>
    /// UIのボタンから呼び出される関数。
    /// INeedleに値を送る。
    /// </summary>
    public void OnClickSend(){
        float value = float.Parse(inputValue.text);

        value = Mathf.Clamp(value, 0.0f, 1.0f);

        needle.SetValue(value);
    }
}
