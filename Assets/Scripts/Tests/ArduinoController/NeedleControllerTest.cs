using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine.TestTools.Utils;
using UnityEngine.TestTools;
using UnityEngine;

public class NeedleControllerTest
{
    /// <summary>
    /// float値を比較する際に許容できる相対誤差の上限
    /// </summary>
    const float allowedRelativeError = 10e-6f;

    [UnityTest]
    public IEnumerator NeedleControllTest()
    {
        // モックのMusicPaseを作成
        var musicPaseGO = new GameObject();
        var musicPase = musicPaseGO.AddComponent<MockMusicPase>();
        TestUtility.CallAwake(musicPase);

        // モックの針を作成
        var needleGO = new GameObject("Needle");
        var needle = needleGO.AddComponent<MockNeedle>();
        TestUtility.CallAwake(needle);

        // NeedleControllerのGameObjectを作成し、コンポーネントを付与する前に針の親GOにする
        var needleControllerGO = new GameObject("NeedleController");
        needleGO.transform.parent = needleControllerGO.transform;

        var needleController = needleControllerGO.AddComponent<NeedleController>();
        TestUtility.CallAwake(needleController);

        // Start関数をまとめて呼び出す。
        // Awake->Startの呼び出し順は全てのGameObejctのAwakeが呼び出されてからStartが呼び出されるので、
        // Awakeを呼び出し終わった後にまとめてStartを実行する必要がある
        TestUtility.CallStart(musicPase);
        TestUtility.CallStart(needle);
        TestUtility.CallStart(needleController);
        yield return null;

        // 曲が本来のテンポで流れている時は、針は真ん中を刺す
        musicPase.CallOnTempoChange(1);

        Assert.That(Utils.AreFloatsEqual(0.5f, needle.CurrentValue, allowedRelativeError));

        yield return null;

        // 曲が本来のテンポの半分で流れている時は、針は低い方に半分の所を刺す

        musicPase.CallOnTempoChange(0.5f);

        Assert.That(Utils.AreFloatsEqual(0.25f, needle.CurrentValue, allowedRelativeError));

        yield return null;

        // 曲が本来のテンポの倍で流れている時は、針は大きい方にマックスの所を刺す

        musicPase.CallOnTempoChange(2.0f);

        Assert.That(Utils.AreFloatsEqual(1.0f, needle.CurrentValue, allowedRelativeError));

        yield return null;

        // 曲が倍速以上のテンポで流れても針は振り切れたまま

        musicPase.CallOnTempoChange(3.0f);

        Assert.That(Utils.AreFloatsEqual(1.0f, needle.CurrentValue, allowedRelativeError));

        yield return null;
    }
}
