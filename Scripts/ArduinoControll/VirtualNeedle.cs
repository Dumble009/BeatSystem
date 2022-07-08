using UnityEngine;

/// <summary>
/// デバッグ・ダーティプロトタイピング用の仮想針
/// </summary>
public class VirtualNeedle : MonoBehaviour, INeedle
{
    /// <summary>
    /// 針の回転中心のTransform
    /// </summary>
    [SerializeField] Transform needleCenter;
    /// <summary>
    /// 針が左端を刺す時の角度
    /// </summary>
    [SerializeField] float leftDegree;
    /// <summary>
    /// 針が右端を刺す時の角度
    /// </summary>
    [SerializeField] float rightDegree;
    public void SetValue(float v)
    {
        float degree = leftDegree + v * (rightDegree - leftDegree); // 0~1の与えられるvを具体的な針の角度に変換する

        var euler = needleCenter.eulerAngles;
        euler.z = degree;

        needleCenter.rotation = Quaternion.Euler(euler);
    }
}
