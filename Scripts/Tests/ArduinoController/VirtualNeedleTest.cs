using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine.TestTools.Utils;
using UnityEngine.TestTools;
using UnityEditor;
using UnityEngine;

public class VirtualNeedleTest
{
    /// <summary>
    /// float値を比較する際に許容できる相対誤差の上限
    /// </summary>
    const float allowedRelativeError = 10e-6f;

    /// <summary>
    /// テスト対象のVirtualNeedleゲームオブジェクト
    /// </summary>
    GameObject virtualNeedleGO;

    /// <summary>
    /// テスト対象のVirtualNeedleコンポーネント。使用する時はINeedle経由で使われることの方が多いはずなのでINeedleで受ける
    /// </summary>
    INeedle needleComponent;

    /// <summary>
    /// テスト対象のVirtualNeedleゲームオブジェクトに含まれる針のTransform
    /// </summary>
    Transform needleCenter;

    /// <summary>
    /// VirtualNeedleプレハブのパス
    /// </summary>
    const string PREFAB_PATH = "Assets/Prefabs/VirtualNeedle.prefab";

    /// <summary>
    /// NeedleCenterゲームオブジェクトの名前
    /// </summary>
    const string NEEDLE_CENTER_NAME = "NeedleCenter";

    /// <summary>
    /// 各テストケースの最初に呼ばれる関数
    /// </summary>
    [SetUp]
    public void Init()
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PREFAB_PATH);
        Assert.That(prefab != null);

        virtualNeedleGO = MonoBehaviour.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        needleComponent = virtualNeedleGO.GetComponent<INeedle>();
        needleCenter = virtualNeedleGO.transform.Find(NEEDLE_CENTER_NAME);
    }

    [UnityTest]
    public IEnumerator LeftEdgeTest()
    {
        yield return null;
    }
}
