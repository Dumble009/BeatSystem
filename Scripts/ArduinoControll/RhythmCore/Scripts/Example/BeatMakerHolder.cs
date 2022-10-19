using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BeatMakerオブジェクトを保持し、他のGameObject等にBeatMakerの機能を提供する
/// </summary>
public class BeatMakerHolder : MonoBehaviour
{
    BeatMaker beatMaker;
    /// <summary>
    /// 拍の管理に使用されるBeatMakerオブジェクトのインスタンス
    /// </summary>
    public BeatMaker Maker
    {
        set
        {
            beatMaker = value;
        }
    }

    private void Awake()
    {
        beatMaker = new SimpleBeatMaker(new UnityTimer());
    }

    /// <summary>
    /// 拍動の発生
    /// </summary>
    public void Beat()
    {
        beatMaker.Beat();
    }

    /// <summary>
    /// 拍動発生時のイベント登録
    /// </summary>
    /// <param name="handler">登録されるイベント</param>
    public void RegisterOnBeat(BeatHandler handler)
    {
        beatMaker.RegisterOnBeat(handler);
    }

    /// <summary>
    /// 拍動発生時のイベントの登録解除
    /// </summary>
    /// <param name="handler">登録解除されるイベント</param>
    public void UnregisterOnBeat(BeatHandler handler)
    {
        beatMaker.UnregisterOnBeat(handler);
    }
}
