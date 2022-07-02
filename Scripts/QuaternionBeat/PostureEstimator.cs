using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// スマホから振ってきたQuaternionの情報を元に姿勢を推定するクラス
/// </summary>
public class QuaternionPostureEstimator
{
    /// <summary>
    /// スマホにおける上向きのベクトル。キャリブレーションではこのベクトルを求めることを目的とする。
    /// </summary>
    static Vector3 upwardOfCell;

    /// <summary>
    /// Unity空間上で各軸にスマホからのQuaternionを掛け合わせた結果得られるベクトル
    /// </summary>
    List<Vector3> rotatedVectorUp, rotatedVectorRight, rotatedVectorForward;
    public void InitCalibration()
    {
        rotatedVectorUp = new List<Vector3>();
        rotatedVectorRight = new List<Vector3>();
        rotatedVectorForward = new List<Vector3>();
    }

    /// <summary>
    /// キャリブレーションを1ステップ進める
    /// </summary>
    /// <param name="rot">スマホから降ってきたQuaternion</param>
    public void StepCalibration(Vector4 rot)
    {
        var unityRot = new Quaternion(rot.x, rot.y, rot.z, rot.w); // スマホからのQuaternionデータをUnity上で扱いやすい構造体としてのQuaternionに変換

        rotatedVectorUp.Add(unityRot * Vector3.up);
        rotatedVectorRight.Add(unityRot * Vector3.right);
        rotatedVectorForward.Add(unityRot * Vector3.forward);
    }

    /// <summary>
    /// キャリブレーションを終了し、情報を更新する
    /// </summary>
    public void FinishCalibration()
    {
        var upError = CalculateRotatedVectorErrors(rotatedVectorUp);
        var rightError = CalculateRotatedVectorErrors(rotatedVectorRight);
        var forwardError = CalculateRotatedVectorErrors(rotatedVectorForward);

        upwardOfCell = GetAxisOfMinimumError(new[] { (upError, Vector3.up), (rightError, Vector3.right), (forwardError, Vector3.forward) });
    }

    /// <summary>
    /// 誤差と各軸のタプルのリストの中から最も誤差の小さな物を選び、その軸のベクトルを返す。resultsに要素が存在しない場合零ベクトルを返す
    /// </summary>
    /// <param name="results">誤差と各軸のベクトルのタプルからなるリスト。1要素目に誤差、2要素目にその軸のベクトルを入れる</param>
    /// <returns>最も誤差が小さかった軸のベクトル</returns>
    Vector3 GetAxisOfMinimumError((float, Vector3)[] results)
    {
        if (results.Length <= 0)
        {
            return Vector3.zero;
        }
        var min = results[0];

        for (int i = 1; i < results.Length; i++)
        {
            if (results[i].Item1 < min.Item1)
            {
                min = results[i];
            }
        }

        return min.Item2;
    }

    /// <summary>
    /// 各軸にQuaternionをかけて得られたベクトル群の間の角度の差の総和を求める。この関数の返り値が小さいほど基準の軸としてふさわしい。
    /// </summary>
    /// <param name="vs">各軸にQuaternionをかけて得られたベクトル群</param>
    /// <returns>ベクトル間の角度の誤差の総和</returns>
    float CalculateRotatedVectorErrors(List<Vector3> vs)
    {
        if (vs.Count <= 0)
        {
            // 不正なリストが与えられた場合は最も悪い値を返す
            return Mathf.Infinity;
        }

        // 0番目のベクトルを基準として、それ以外のベクトルと0番目のベクトルとの間の角度の誤差の総和を計算する。
        float ans = 0.0f;
        var baseVec = vs[0];

        for (int i = 1; i < vs.Count; i++)
        {
            ans += Vector3.Angle(baseVec, vs[i]);
        }

        return ans;
    }
}
