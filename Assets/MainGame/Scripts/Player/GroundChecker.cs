using UnityEngine;

/// <summary>
/// 地面判定を行うクラス。
/// 横幅を持つ矩形（OverlapBox）で検知するため、プラットフォームの端でも確実に判定できる。
/// </summary>
public class GroundChecker : MonoBehaviour
{
    [SerializeField]
    [Tooltip("地面判定の基点となるTransform（プレイヤー足元の中心に設定する）")]
    private Transform groundCheck;

    [SerializeField]
    [Tooltip("地面判定ボックスの横幅（プレイヤーのコライダー幅より少し小さい値を推奨）")]
    private float checkWidth = 0.8f;

    [SerializeField]
    [Tooltip("地面判定ボックスの高さ（小さい値で足元のみを判定する）")]
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
