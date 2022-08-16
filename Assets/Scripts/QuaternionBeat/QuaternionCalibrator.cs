using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Quaternionを使用する際のキャリブレーションの進行を行うクラス
/// </summary>
public class QuaternionCalibrator : MonoBehaviour
{
    /// <summary>
    /// キャリブレーションの指示が表示されるTextUI
    /// </summary>
    [SerializeField] Text description;

    /// <summary>
    /// キャリブレーション完了後に読み込まれるシーンの名前
    /// </summary>
    [SerializeField] string loadSceneName;

    /// <summary>
    /// 姿勢推定を行うオブジェクト。キャリブレーション対象。
    /// </summary>
    QuaternionPostureEstimator postureEstimator;

    /// <summary>
    /// 現在のスマホの姿勢
    /// </summary>
    Vector4 currentCellRotation;
    IEnumerator Start()
    {
        postureEstimator = new QuaternionPostureEstimator();

        postureEstimator.InitCalibration();

        ChangeDescription("Stand the Smartphone");
        yield return WaitForKey(KeyCode.Return);

        postureEstimator.StepCalibration(currentCellRotation);

        ChangeDescription("Rotate 90 degrees around a vertical axis.");
        yield return WaitForKey(KeyCode.Return);

        postureEstimator.StepCalibration(currentCellRotation);

        postureEstimator.FinishCalibration();

        ChangeDescription("Calibration Completed!");
        SceneManager.LoadScene(loadSceneName);
    }

    /// <summary>
    /// キーが押されるまで待つ。1フレーム間で1つしか通らない。
    /// </summary>
    /// <param name="code">検出対象のキーコード</param>
    /// <returns>待機コルーチン</returns>
    IEnumerator WaitForKey(KeyCode code)
    {
        yield return new WaitUntil(() => Input.GetKeyDown(code));
        yield return new WaitUntil(() => Input.GetKeyUp(code));
    }

    /// <summary>
    /// 説明文を更新する
    /// </summary>
    /// <param name="msg">新たな説明文</param>
    void ChangeDescription(string msg)
    {
        description.text = msg;
    }

    /// <summary>
    /// OSC通信のハンドラ
    /// </summary>
    /// <param name="rot">スマホのQuaternion</param>
    public void Receive(Vector4 rot)
    {
        currentCellRotation = rot;
    }
}
