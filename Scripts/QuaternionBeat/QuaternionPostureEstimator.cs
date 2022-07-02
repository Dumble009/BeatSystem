using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// スマホから振ってきたQuaternionの情報を元に姿勢を推定するクラス。
/// このクラスは現在のスマホからのQuaternionを用いて、現在のスマホの上方向のベクトルが鉛直上向きから何度離れているのかを調べることが出来る。
/// 事前にキャリブレーションが必要であり、キャリブレーションによってスマホ空間上における上方向のベクトルと、スマホを鉛直に立てた時に上方向がどこへいくかを推定する。
/// </summary>
public class QuaternionPostureEstimator
{
    /// <summary>
    /// スマホにおける上向きのベクトル。キャリブレーションではこのベクトルを求めることを目的とする。
    /// </summary>
    static Vector3 upwardOfCell;

    /// <summary>
    /// スマホを垂直に立てた時にスマホの上側が来るベクトル。
    /// </summary>
    static Vector3 verticalUp;

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
        var unityRot = Vector4ToQuaternion(rot); // スマホからのQuaternionデータをUnity上で扱いやすい構造体としてのQuaternionに変換

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

        var result = GetCalibrationDataOfMinimumError(new[] { new CalibrationData(upError, Vector3.up, rotatedVectorUp[0]),
                                        new CalibrationData(rightError, Vector3.right, rotatedVectorRight[0]),
                                        new CalibrationData(forwardError, Vector3.forward, rotatedVectorForward[0]) });

        upwardOfCell = result.Axis;
        verticalUp = result.Vertical;
    }

    /// <summary>
    /// 現在のスマホの上方向のベクトルと、真上との角度の差(デグリー)を求めて返す
    /// </summary>
    /// <param name="rot">OSCから降ってきたスマホのQuaternion</param>
    /// <returns>現在のスマホの上方向のベクトルと、真上との角度の差(デグリー)</returns>
    public float ErrorBetweenUpward(Vector4 rot)
    {
        var currentUpward = Vector4ToQuaternion(rot) * upwardOfCell;
        return Vector3.Angle(currentUpward, verticalUp); // 現在のスマホの上方向のベクトルと鉛直時に上方向が来るはずのベクトルの間の角度差を計算する
    }

    /// <summary>
    /// CalibrationDataのリストの中から最も誤差の小さな物を選んで返す。resultsに要素が存在しない場合nullを返す
    /// </summary>
    /// <param name="results">CalibrationDataのリスト</param>
    /// <returns>最も誤差が小さいCalibrationData</returns>
    CalibrationData GetCalibrationDataOfMinimumError(CalibrationData[] results)
    {
        if (results.Length <= 0)
        {
            return null;
        }
        var min = results[0];

        for (int i = 1; i < results.Length; i++)
        {
            if (results[i].Error < min.Error)
            {
                min = results[i];
            }
        }

        return min;
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

    /// <summary>
    /// Vector4を同じx,y,z,wを持つQuaternionに変換する
    /// </summary>
    /// <param name="v">変換対象のVector4構造体</param>
    /// <returns>vと同じパラメータを持つQuaternion</returns>
    Quaternion Vector4ToQuaternion(Vector4 v)
    {
        return new Quaternion(v.x, v.y, v.z, v.w);
    }
}

/// <summary>
/// スマホから見て上方向がどの軸として見えているかを求める際に使用する情報をまとめて保持する
/// </summary>
class CalibrationData
{

    float error;
    /// <summary>
    /// キャリブレーション操作中に生じた各軸の誤差
    /// </summary>
    public float Error
    {
        get
        {
            return error;
        }
    }

    Vector3 axis;
    /// <summary>
    /// 現在対象としている軸のベクトル
    /// </summary>
    public Vector3 Axis
    {
        get
        {
            return axis;
        }
    }

    Vector3 vertical;
    /// <summary>
    /// スマホを鉛直に立てた時にスマホから相対的に見たその軸方向のベクトルがスマホのワールド空間のどこへ行くか
    /// </summary>
    public Vector3 Vertical
    {
        get
        {
            return vertical;
        }
    }

    public CalibrationData(float e, Vector3 a, Vector3 v)
    {
        error = e;
        axis = a;
        vertical = v;
    }
}