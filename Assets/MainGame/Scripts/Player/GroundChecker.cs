using UnityEngine;

/// <summary>
/// 地面判定を行うクラス
/// </summary>
public class GroundChecker : MonoBehaviour
{
    [SerializeField]
    [Tooltip("地面の判定を行うトランスフォーム")]
    private Transform groundCheck;

    [SerializeField]
    [Tooltip("地面の判定を行う半径")]
    private float groundCheckRadius = 0.2f;

    [SerializeField]
    [Tooltip("地面と判定するレイヤー")]
    private LayerMask groundLayer;

    /// <summary>
    /// 地面に接地しているかどうかを示すプロパティ
    /// </summary>
    public bool IsGrounded { get; private set; }

    void Update()
    {
        // 地面に接地しているかどうかを判定
        IsGrounded = Physics2D.OverlapCircle(
            groundCheck.position, 
            groundCheckRadius, 
            groundLayer);
    }
}
