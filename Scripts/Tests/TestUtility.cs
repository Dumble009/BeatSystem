using System.Reflection;
using UnityEngine;

/// <summary>
/// テストで共通して使用される処理を提供するユーティリティクラス
/// </summary>
public class TestUtility
{
    /// <summary>
    /// MonoBehaviour m に含まれるnameという名前の関数の情報を返す
    /// </summary>
    /// <param name="m">対象のMonoBehaviour</param>
    /// <param name="name">捜索対象の関数</param>
    /// <returns>関数nameの情報</returns>
    static private MethodInfo GetMethodInfo(MonoBehaviour m, string name)
    {
        return m.GetType().GetMethod(name, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
    }

    /// <summary>
    /// 引数として与えられたMonoBehaviour mのAwake関数を呼び出す
    /// </summary>
    /// <param name="m">Awakeを呼び出すMonoBehaviour</param>
    static public void CallAwake(MonoBehaviour m)
    {
        var mi = GetMethodInfo(m, "Awake");
        if (mi != null)
        {
            mi.Invoke(m, null);
        }
    }

    /// <summary>
    /// 引数として与えられたMonoBehaviour mのStart関数を呼び出す
    /// </summary>
    /// <param name="m">Startを呼び出すMonoBehaviour</param>
    static public void CallStart(MonoBehaviour m)
    {
        var mi = GetMethodInfo(m, "Start");
        if (mi != null)
        {
            mi.Invoke(m, null);
        }
    }
}
