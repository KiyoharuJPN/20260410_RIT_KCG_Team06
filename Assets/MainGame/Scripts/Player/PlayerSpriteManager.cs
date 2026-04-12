using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのスプライトをステートと向きに応じて切り替えるクラス。
/// </summary>
public class PlayerSpriteManager : MonoBehaviour
{
    // スプライトリストのインデックス定数
    private const int IDX_IDLE_RIGHT     = 0;
    private const int IDX_IDLE_LEFT      = 1;
    private const int IDX_CHARGE_RIGHT   = 2;
    private const int IDX_CHARGE_LEFT    = 3;
    private const int IDX_JUMP_RIGHT     = 4;
    private const int IDX_JUMP_LEFT      = 5;
    private const int IDX_PUNCH_RIGHT    = 6;
    private const int IDX_PUNCH_LEFT     = 7;
    private const int IDX_PUNCH_UP_RIGHT = 8;
    private const int IDX_PUNCH_UP_LEFT  = 9;
    private const int IDX_FALL_RIGHT     = 10;
    private const int IDX_FALL_LEFT      = 11;
    private const int SPRITE_TOTAL       = 12;

    /// <summary>
    /// スプライトを表示するSpriteRenderer
    /// </summary>
    [SerializeField, Tooltip("スプライトを変更するSpriteRenderer")]
    private SpriteRenderer m_spriteRenderer;

    /// <summary>
    /// プレイヤーの状態機械
    /// </summary>
    [SerializeField, Tooltip("PlayerStateMachineコンポーネント")]
    private PlayerStateMachine m_stateMachine;

    /// <summary>
    /// パンチ実行クラスへの参照
    /// </summary>
    [SerializeField, Tooltip("PlayerPunchExecutorコンポーネント")]
    private PlayerPunchExecutor m_punchExecutor;

    /// <summary>
    /// ステートと向きに対応するスプライトリスト
    /// </summary>
    [SerializeField]
    private List<Sprite> m_sprites = new List<Sprite>();

    /// <summary>
    /// 現在の向き
    /// </summary>
    private bool m_facingRight = true;

    private void Awake()
    {
        // 省略時は同一オブジェクトから自動取得
        if (m_spriteRenderer == null)  m_spriteRenderer  = GetComponent<SpriteRenderer>();
        if (m_stateMachine    == null)  m_stateMachine    = GetComponent<PlayerStateMachine>();
        if (m_punchExecutor   == null)  m_punchExecutor   = GetComponent<PlayerPunchExecutor>();

        // 必須コンポーネントチェック
        if (m_spriteRenderer == null) Debug.LogError("PlayerSpriteManager: SpriteRendererが見つかりません");
        if (m_stateMachine   == null) Debug.LogError("PlayerSpriteManager: PlayerStateMachineが見つかりません");

        // スプライト枚数のバリデーション
        if (m_sprites.Count < SPRITE_TOTAL)
        {
            Debug.LogWarning($"PlayerSpriteManager: スプライトが{SPRITE_TOTAL}枚必要ですが、{m_sprites.Count}枚しか設定されていません");
        }
    }

    private void Start()
    {
        // 初期ステートのスプライトを即座に適用
        ApplySprite(GetCurrentSpriteIndex());
    }

    private void LateUpdate()
    {
        // パンチ中は向きを更新してパンチスプライトを優先適用
        if (m_punchExecutor != null && m_punchExecutor.IsPunching)
        {
            // 左右パンチのときのみ向きを更新（上パンチは向き変更なし）
            var punchDir = m_punchExecutor.LastPunchDirection;
            if (punchDir == PlayerPunchExecutor.PunchDirection.Right) m_facingRight = true;
            if (punchDir == PlayerPunchExecutor.PunchDirection.Left)  m_facingRight = false;
        }

        ApplySprite(GetCurrentSpriteIndex());
    }

    /// <summary>
    /// 現在のステートと向きから適用すべきスプライトのインデックスを返す
    /// </summary>
    private int GetCurrentSpriteIndex()
    {
        // パンチ中はパンチスプライトを優先
        if (m_punchExecutor != null && m_punchExecutor.IsPunching)
        {
            var punchDir = m_punchExecutor.LastPunchDirection;

            // 上パンチは向きを変更せず、現在の向きに応じたUp_Right/Up_Leftを使用
            if (punchDir == PlayerPunchExecutor.PunchDirection.Up)
            {
                return m_facingRight ? IDX_PUNCH_UP_RIGHT : IDX_PUNCH_UP_LEFT;
            }

            return punchDir switch
            {
                PlayerPunchExecutor.PunchDirection.Right => IDX_PUNCH_RIGHT,
                PlayerPunchExecutor.PunchDirection.Left  => IDX_PUNCH_LEFT,
                _                                        => IDX_PUNCH_RIGHT
            };
        }

        // 向きのオフセット（右=0、左=1）
        int dirOffset = m_facingRight ? 0 : 1;

        // ステートに応じたインデックスを返す
        return m_stateMachine.CurrentState switch
        {
            PlayerState.Idle        => IDX_IDLE_RIGHT   + dirOffset,
            PlayerState.Charging    => IDX_CHARGE_RIGHT + dirOffset,
            PlayerState.ChargeReady => IDX_CHARGE_RIGHT + dirOffset, // Chargingと共用
            PlayerState.Jumping     => IDX_JUMP_RIGHT   + dirOffset,
            PlayerState.Falling     => IDX_FALL_RIGHT   + dirOffset,
            _                       => IDX_IDLE_RIGHT   + dirOffset
        };
    }

    /// <summary>
    /// 指定インデックスのスプライトをSpriteRendererに適用する
    /// </summary>
    private void ApplySprite(int index)
    {
        if (m_spriteRenderer == null) return;
        if (m_sprites == null || index >= m_sprites.Count)
        {
            Debug.LogWarning($"PlayerSpriteManager: インデックス {index} のスプライトが未登録です");
            return;
        }

        Sprite sprite = m_sprites[index];
        if (sprite == null)
        {
            Debug.LogWarning($"PlayerSpriteManager: インデックス {index} のスプライトがnullです");
            return;
        }

        // 前フレームと同じスプライトなら更新をスキップ
        if (m_spriteRenderer.sprite == sprite) return;
        m_spriteRenderer.sprite = sprite;
    }
}
