using UnityEngine;

/// <summary>
/// 実際にプレイヤーをジャンプさせるクラス
/// </summary>
public class PlayerJumpExecutor : MonoBehaviour
{
    [SerializeField]
    [Tooltip("プレイヤーのRigidbody2Dコンポーネント")]
    private Rigidbody2D playerRigidbody;

    [SerializeField]
    [Tooltip("マスターデータ")]
    private GameMasterData masterData;

    /// <summary>
    /// ジャンプした際の時間を記録する用
    /// </summary>
    private float lastJumpTime;

    /// <summary>
    /// ジャンプを実行するメソッド
    /// </summary>
    public void Jump(float holdTime, bool isBonus)
    {
        // クールタイム中はジャンプ不可
        if (Time.time - lastJumpTime < masterData.JumpCooldown)
        {
            return;
        }

        // 長押し時間でジャンプ力を決定
        // 0.5秒未満は小ジャンプ
        // 0.5秒以上は大ジャンプ
        float jumpPower = holdTime >= 0.5f 
            ? masterData.LargeJumpPower
            : masterData.SmallJumpPower;

        // ボーナスステージの場合はジャンプ力を上昇させる
        if (isBonus)
        {
            jumpPower *= 1.5f;
        }

        Debug.Log("Jump Executed");

        // ジャンプ力を適用
        playerRigidbody.linearVelocity = new Vector2(playerRigidbody.linearVelocity.x, jumpPower);

        // 上昇中は重力を小さくして一気に上昇させる
        playerRigidbody.gravityScale = masterData.JumpGravityScale;

        // 最後のジャンプ時間を更新
        lastJumpTime = Time.time;
    }
}
