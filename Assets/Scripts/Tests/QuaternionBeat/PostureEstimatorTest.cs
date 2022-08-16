using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;
using UnityEngine.TestTools;

/// <summary>
/// PostureEstimatorの動作を確認するためのテスト
/// </summary>
public class PostureEstimatorTest
{
    /// <summary>
    /// テスト対象のオブジェクト
    /// </summary>
    QuaternionPostureEstimator estimator;

    /// <summary>
    /// float値を比較する際に許容できる相対誤差の上限
    /// </summary>
    const float allowedRelativeError = 10e-6f;

    /// <summary>
    /// 各テストの直前に呼び出され変数の初期化等を行う
    /// </summary>
    [SetUp]
    public void Init()
    {
        estimator = new QuaternionPostureEstimator();
    }

    /// <summary>
    /// 立っている状態が基準姿勢であり、基準姿勢でスマホから見て上側がy軸になるテスト
    /// </summary>
    [Test]
    public void CalibrationUpTest_StandIdentity()
    {
        estimator.InitCalibration();

        StandIdentityTest(Vector3.up);
    }

    /// <summary>
    /// 立っている状態が基準姿勢であり、基準姿勢でスマホから見て上側がx軸になるテスト
    /// </summary>
    [Test]
    public void CalibrationRightTest_StandIdentity()
    {
        estimator.InitCalibration();

        StandIdentityTest(Vector3.right);
    }

    /// <summary>
    /// 立っている状態が基準姿勢であり、基準姿勢でスマホから見て上側がz軸になるテスト
    /// </summary>
    [Test]
    public void CalibrationForwardTest_StandIdentity()
    {
        estimator.InitCalibration();

        StandIdentityTest(Vector3.forward);
    }

    /// <summary>
    /// 寝かした状態が基準姿勢であり、基準姿勢でスマホから見て上側がy軸、右側がx軸になるテスト
    /// </summary>
    [Test]
    public void CalibrationUpTest_LayIdentity()
    {
        estimator.InitCalibration();

        LayIdentityTest(Vector3.right, Vector3.up);
    }

    /// <summary>
    /// 寝かした状態が基準姿勢であり、基準姿勢でスマホから見て上側がx軸、右側がz軸になるテスト
    /// </summary>
    [Test]
    public void CalibrationRightTest_LayIdentity()
    {
        estimator.InitCalibration();

        LayIdentityTest(Vector3.up, Vector3.forward);
    }

    /// <summary>
    /// 寝かした状態が基準姿勢であり、基準姿勢でスマホから見て上側がz軸、右側がy軸になるテスト
    /// </summary>
    [Test]
    public void CalibrationForwardTest_LayIdentity()
    {
        estimator.InitCalibration();

        LayIdentityTest(Vector3.forward, Vector3.up);
    }

    void StandIdentityTest(Vector3 axis)
    {
        // キャリブレーション結果がaxisとなる→スマホの座標系において上方向がaxisとなっている。
        // 立っている状態が基準姿勢となることを想定しているので、基準姿勢から上下方向(y軸)中心に適当に回転させた状態を入力としている

        var postureOfCell = Quaternion.AngleAxis(45.0f, axis);
        estimator.StepCalibration(QuaternionToVector4(postureOfCell));

        postureOfCell = Quaternion.AngleAxis(70.0f, axis);
        estimator.StepCalibration(QuaternionToVector4(postureOfCell));

        estimator.FinishCalibration();

        postureOfCell = Quaternion.AngleAxis(60.0f, axis);
        var estimatedError = estimator.ErrorBetweenUpward(QuaternionToVector4(postureOfCell));
        Assert.That(Utils.AreFloatsEqual(0.0f, estimatedError, allowedRelativeError));
    }


    void LayIdentityTest(Vector3 horizontalAxis, Vector3 verticalAxis)
    {
        var toStand = Quaternion.AngleAxis(90.0f, verticalAxis);

        var postureOfCell = toStand * Quaternion.AngleAxis(45.0f, verticalAxis);
        estimator.StepCalibration(QuaternionToVector4(postureOfCell));

        postureOfCell = toStand * Quaternion.AngleAxis(70.0f, verticalAxis);
        estimator.StepCalibration(QuaternionToVector4(postureOfCell));

        estimator.FinishCalibration();

        postureOfCell = toStand * Quaternion.AngleAxis(60.0f, verticalAxis);
        var estimatedError = estimator.ErrorBetweenUpward(QuaternionToVector4(postureOfCell));
        Assert.That(Utils.AreFloatsEqual(0.0f, estimatedError, allowedRelativeError));
    }

    Vector4 QuaternionToVector4(Quaternion q)
    {
        return new Vector4(q.x, q.y, q.z, q.w);
    }
}
