using UnityEngine;

/// <summary>
/// キーを押した際に拍動を起こすコンポーネント
/// </summary>
public class KeyBeater : MonoBehaviour
{
    /// <summary>
    /// 拍動を管理してくれるオブジェクト
    /// </summary>
    [SerializeField] BeatMakerHolder beatMakerHolder;

    private void Update()
    {
        // スペースキーを押したら拍動を起こす
        if (Input.GetKeyDown(KeyCode.Space))
        {
            beatMakerHolder.Beat();
        }
    }
}
