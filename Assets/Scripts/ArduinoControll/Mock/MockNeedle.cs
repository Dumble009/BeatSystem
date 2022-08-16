using UnityEngine;

/// <summary>
/// テストで使用するための針のモック
/// </summary>
public class MockNeedle : MonoBehaviour, INeedle
{

    float currentValue;
    /// <summary>
    /// 今針が刺している値
    /// </summary>
    public float CurrentValue
    {
        get
        {
            return currentValue;
        }
    }

    public void SetValue(float v)
    {
        currentValue = v;
    }
}
