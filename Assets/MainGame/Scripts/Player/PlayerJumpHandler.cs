using UnityEngine;

/// <summary>
/// プレイヤーのジャンプ入力を判定するクラス
/// </summary>
public class PlayerJumpHandler : MonoBehaviour
{
    /// <summary>
    /// ジャンプキーが押されている時間
    /// </summary>
    private float pressTime;

    /// <summary>
    /// キーを長押ししているか
    /// </summary>
    private bool isHolding;

    /// <summary>
    /// 長押した時間を取得するプロパティ
    /// </summary>
    public float HoldDuration { get; private set; }

    /// <summary>
    /// 長押し時間を計る
    /// </summary>
    public void StartHold()
    {
        // フラグを立てる
        isHolding = true;

        // 押した時間を加算
        pressTime = Time.time;
    }

    public void EndHold()
    {
        // フラグを下ろす
        isHolding = false;

        // 長押し時間を計算
        HoldDuration = Time.time - pressTime;
    }
}
