using UnityEngine;

/// <summary>
/// OSC経由で送られてくる姿勢データに基づいてビートを刻む
/// </summary>
public class OSCQuaternionBeater : MonoBehaviour
{
    [SerializeField] BeatMakerHolder holder;
    /// <summary>
    /// 持ち上げるときの閾値
    /// </summary>
    [SerializeField] float upperThreshold = 0.7f;

    /// <summary>
    /// 下げるときの閾値
    /// </summary>
    [SerializeField] float downerThreshold = -0.4f;

    /// <summary>
    /// 今持ち上げるべきかどうか
    /// </summary>
    bool isRising = true;

    public void Receive(Vector4 rot)
    {
        if (isRising)
        {
            if (IsRiftedUp(rot))
            {
                holder.Beat();
                isRising = false;
                Debug.LogWarning("rifted up");
            }
        }
        else
        {
            if (IsRiftedDown(rot))
            {
                holder.Beat();
                isRising = true;
                Debug.LogWarning("rifted down");
            }
        }

        Debug.Log(rot + ":" + rot.magnitude);
    }

    /// <summary>
    /// ダンベルを持ち上げたかどうか
    /// </summary>
    /// <param name="rot">OSCから降ってきた姿勢データ</param>
    /// <returns>ダンベルを持ち上げたかどうか</returns>
    private bool IsRiftedUp(Vector4 rot)
    {
        return rot.x > upperThreshold;
    }

    /// <summary>
    /// ダンベルを下げたかどうか
    /// </summary>
    /// <param name="rot">OSCから降ってきた姿勢データ</param>
    /// <returns>ダンベルを下げたかどうか</returns>
    private bool IsRiftedDown(Vector4 rot)
    {
        return rot.x < downerThreshold;
    }
}
