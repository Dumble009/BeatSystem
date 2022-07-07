
/// <summary>
/// テストで使用するMusicPaseのモッククラス
/// </summary>
public class MockMusicPase : MusicPase
{
    /// <summary>
    /// argを引数としてonTempoChangeを呼び出す
    /// </summary>
    /// <param name="arg">onTempoChangeに渡される引数</param>
    public void CallOnTempoChange(float arg)
    {
        onTempoChange(arg);
    }
}
