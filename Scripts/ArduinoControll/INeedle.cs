/// <summary>
/// 針を操作するクラスに共通のクラス
/// </summary>
public interface INeedle
{
    /// <summary>
    /// 針の示す値を変更する。
    /// </summary>
    /// <param name="v">針が示す値。下端が0、上端が1</param>
    public void SetValue(float v);
}
