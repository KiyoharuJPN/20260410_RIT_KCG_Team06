using UnityEngine;

/// <summary>
/// 地面判定を行うクラス。
/// </summary>
public class GroundChecker : MonoBehaviour
{
    [SerializeField]
    [Tooltip("地面判定の基点となるTransform")]
    private Transform groundCheck;

    [SerializeField]
    [Tooltip("地面判定ボックスの横幅")]
    private float checkWidth = 0.8f;

    [SerializeField]
    [Tooltip("地面判定ボックスの高さ")]
    private float checkHeight = 0.1f;

    [SerializeField]
    [Tooltip("地面と判定するレイヤー")]
    private LayerMask groundLayer;

    /// <summary>
    /// 地面に接地しているかどうかを示すプロパティ
    /// </summary>
    public bool IsGrounded { get; private set; }

    void Update()
    {
        // 横幅を持つ矩形で地面を判定することで、プラットフォームの端でも検知できる
        IsGrounded = Physics2D.OverlapBox(
            groundCheck.position,
            new Vector2(checkWidth, checkHeight),
            0f,
            groundLayer);
    }

    /// <summary>
    /// Sceneビューで判定範囲を可視化するためのデバッグ描画
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        // 接地中は緑、非接地は赤で表示
        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Gizmos.DrawWireCube(
            groundCheck.position,
            new Vector3(checkWidth, checkHeight, 0f));
    }
}
