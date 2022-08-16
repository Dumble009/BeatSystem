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
    const string PREFAB_PATH = "Assets/BeatSystem/Prefabs/VirtualNeedle.prefab";

    /// <summary>
    /// NeedleCenterゲームオブジェクトの名前
    /// </summary>
    const string NEEDLE_CENTER_NAME = "NeedleCenter";

    /// <summary>
    /// VirtualNeedleコンポーネントの変数leftDegreeの名前
    /// </summary>
    const string LEFT_DEGREE_NAME = "leftDegree";
    /// <summary>
    /// VirtualNeedleコンポーネントの変数leftDegreeの名前
    /// </summary>
    const string RIGHT_DEGREE_NAME = "rightDegree";

    /// <summary>
    /// 各テストケースの最初に呼ばれる関数
    /// </summary>
    [SetUp]
    public void Init()
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PREFAB_PATH);
        Assert.AreNotEqual(null, prefab);

        virtualNeedleGO = MonoBehaviour.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        Assert.AreNotEqual(null, virtualNeedleGO);

        needleComponent = virtualNeedleGO.GetComponent<INeedle>();
        Assert.AreNotEqual(null, needleComponent);

        needleCenter = virtualNeedleGO.transform.Find(NEEDLE_CENTER_NAME);
        Assert.AreNotEqual(null, needleCenter);

        TestUtility.CallAwake(needleComponent as MonoBehaviour);
        TestUtility.CallStart(needleComponent as MonoBehaviour);
    }

    /// <summary>
    /// 左端限界まで針が振れるかのテスト
    /// </summary>
    [UnityTest]
    public IEnumerator LeftEdgeTest()
    {
        needleComponent.SetValue(0.0f); // 左端は0

        float needleDegree = GetNeedleDegree();

        Assert.AreEqual(GetLeftDegree(), needleDegree, allowedRelativeError);

        yield return null;
    }

    /// <summary>
    /// 左方向の中間値に針が振れるかのテスト
    /// </summary>
    [UnityTest]
    public IEnumerator LeftMidTest()
    {
        needleComponent.SetValue(0.25f); // 左端が0、中間が0.5なので左方向の中間は0.25

        float needleDegree = GetNeedleDegree();

        Assert.AreEqual(GetLeftDegree() / 2.0f, needleDegree, allowedRelativeError);
        yield return null;
    }

    /// <summary>
    /// 右端限界まで針が振れるかのテスト
    /// </summary>
    [UnityTest]
    public IEnumerator RightEdgeTest()
    {
        needleComponent.SetValue(1.0f); // 右端は1

        float needleDegree = GetNeedleDegree();

        Assert.AreEqual(GetRightDegree(), needleDegree, allowedRelativeError);

        yield return null;
    }

    /// <summary>
    /// 右方向の中間値に針が振れるかのテスト
    /// </summary>
    [UnityTest]
    public IEnumerator RightMidTest()
    {
        needleComponent.SetValue(0.75f); // 右端が1、中間が0.5なので右方向の中間は0.75

        float needleDegree = GetNeedleDegree();


        Assert.AreEqual(GetRightDegree() / 2.0f, needleDegree, allowedRelativeError);
        yield return null;
    }

    /// <summary>
    /// 全体の真ん中に針が振れるかのテスト
    /// </summary>
    /// <returns></returns>
    public IEnumerator CenterTest()
    {
        needleComponent.SetValue(0.5f); // 全体の中間は0.5

        float needleDegree = GetNeedleDegree();

        Assert.AreEqual(0.0f, needleDegree, allowedRelativeError);
        yield return null;
    }

    /// <summary>
    /// 現在の針のオイラー角のZ軸周りの角度を-180~180の範囲で
    /// </summary>
    /// <returns>Z軸周りの角度(デグリー)。-180~180の範囲</returns>
    private float GetNeedleDegree()
    {
        float zDegree = needleCenter.transform.eulerAngles.z;
        return zDegree <= 180.0f ? zDegree : (zDegree - 360.0f);
    }

    /// <summary>
    /// VirtualNeedleコンポーネントのメンバ変数leftDegreeの値を返す
    /// </summary>
    /// <returns>leftDegreeの値</returns>
    private float GetLeftDegree()
    {
        return TestUtility.GetMemberValue<float>(needleComponent as Object, LEFT_DEGREE_NAME);
    }

    /// <summary>
    /// VirtualNeedleコンポーネントのメンバ変数rightDegreeの値を返す
    /// </summary>
    /// <returns>rightDegreeの値</returns>
    private float GetRightDegree()
    {
        return TestUtility.GetMemberValue<float>(needleComponent as Object, RIGHT_DEGREE_NAME);
    }
}
