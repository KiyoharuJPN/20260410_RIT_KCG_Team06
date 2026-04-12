using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの状態（PlayerState）に応じてスプライトを切り替えるクラス。
/// スプライトリストの要素番号がPlayerStateの値（enum順）と対応する。
/// </summary>
/// <remarks>
/// スプライトの登録順（インスペクターで設定）:
///   Element 0 → Idle
///   Element 1 → Charging（地面でチャージ中）
///   Element 2 → ChargeReady（チャージ確定、再押下待機）
///   Element 3 → Jumping（上昇中）
///   Element 4 → Falling（落下中）
/// </remarks>
public class PlayerSpriteManager : MonoBehaviour
{
    /// <summary>
    /// スプライトを表示するSpriteRenderer（未設定なら自身からGetComponent）
    /// </summary>
    [SerializeField, Tooltip("スプライトを変更するSpriteRenderer（省略時は自身のコンポーネントを使用）")]
    private SpriteRenderer m_spriteRenderer;

    /// <summary>
    /// ステートを参照するPlayerStateMachine（未設定なら自身からGetComponent）
    /// </summary>
    [SerializeField, Tooltip("PlayerStateMachineコンポーネント（省略時は自身のコンポーネントを使用）")]
    private PlayerStateMachine m_playerStateMachine;

    /// <summary>
    /// 各ステートに対応するスプライト一覧。
    /// インデックス番号がPlayerStateのenum値と一致する。
    /// </summary>
    [SerializeField, Tooltip(
        "ステートと対応するスプライト一覧\n" +
        "Element 0 : Idle\n" +
        "Element 1 : Charging\n" +
        "Element 2 : ChargeReady\n" +
        "Element 3 : Jumping\n" +
        "Element 4 : Falling")]
    private List<Sprite> m_sprites = new List<Sprite>();

    /// <summary>
    /// 前フレームのステート（変化があったときのみスプライトを更新するために保持）
    /// </summary>
    private PlayerState m_prevState;

    private void Awake()
    {
        // 未アタッチの場合は同一オブジェクトから自動取得
        if (m_spriteRenderer == null)
        {
            m_spriteRenderer = GetComponent<SpriteRenderer>();
        }
        if (m_playerStateMachine == null)
        {
            m_playerStateMachine = GetComponent<PlayerStateMachine>();
        }

        // 必須コンポーネントが見つからない場合は警告
        if (m_spriteRenderer == null)
        {
            Debug.LogError("PlayerSpriteManager: SpriteRendererが見つかりません");
        }
        if (m_playerStateMachine == null)
        {
            Debug.LogError("PlayerSpriteManager: PlayerStateMachineが見つかりません");
        }
    }

    private void Start()
    {
        // 起動時に初期ステートのスプライトを即座に適用
        m_prevState = m_playerStateMachine.CurrentState;
        ApplySprite(m_prevState);
    }

    private void LateUpdate()
    {
        PlayerState currentState = m_playerStateMachine.CurrentState;

        // 前フレームと同じ状態であればスプライトの更新をスキップ
        if (currentState == m_prevState)
        {
            return;
        }

        // ステートが変わったのでスプライトを切り替え
        m_prevState = currentState;
        ApplySprite(currentState);
    }

    /// <summary>
    /// 指定したステートに対応するスプライトをSpriteRendererに適用します。
    /// </summary>
    /// <param name="state">適用するPlayerState</param>
    private void ApplySprite(PlayerState state)
    {
        // enum値をそのままリストのインデックスとして使用
        int index = (int)state;

        // インデックスが範囲外の場合は警告を出してスキップ
        if (m_sprites == null || index >= m_sprites.Count)
        {
            Debug.LogWarning($"PlayerSpriteManager: インデックス {index}（{state}）に対応するスプライトが未登録です");
            return;
        }

        Sprite sprite = m_sprites[index];

        // エレメント自体はあるがスプライトがnullの場合も警告
        if (sprite == null)
        {
            Debug.LogWarning($"PlayerSpriteManager: インデックス {index}（{state}）のスプライトがnullです");
            return;
        }

        m_spriteRenderer.sprite = sprite;
        Debug.Log($"PlayerSpriteManager: スプライト変更 → {state}（index={index}）");
    }
}
