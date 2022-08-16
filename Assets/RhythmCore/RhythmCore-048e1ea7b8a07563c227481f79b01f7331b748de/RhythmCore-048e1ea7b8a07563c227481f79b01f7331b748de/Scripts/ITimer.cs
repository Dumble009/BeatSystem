
/// <summary>
/// テンポの計算等に使用するための現在時間を提供するクラスが備えるインタフェース
/// </summary>
public interface ITimer
{
    /// <summary>
    /// 現在時間を返す
    /// </summary>
    /// <returns>起動時からの経過秒数</returns>
    float GetCurrentTime();
}