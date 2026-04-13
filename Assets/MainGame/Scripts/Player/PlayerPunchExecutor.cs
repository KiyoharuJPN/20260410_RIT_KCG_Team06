using AudioName;
using System.Collections;
using UnityEngine;

/// <summary>
/// プレイヤーの空中パンチ攻撃を管理するクラス。
/// </summary>
public class PlayerPunchExecutor : MonoBehaviour
{
    [SerializeField]
    [Tooltip("左方向のパンチ判定オブジェクト")]
    private GameObject m_leftHitbox;

    [SerializeField]
    [Tooltip("右方向のパンチ判定オブジェクト")]
    private GameObject m_rightHitbox;

    [SerializeField]
    [Tooltip("上方向のパンチ判定オブジェクト")]
    private GameObject m_upHitbox;

    [SerializeField]
    [Tooltip("パンチ判定の持続時間")]
    private float m_hitboxDuration = 0.1f;

    /// <summary>
    /// パンチの方向を示す列挙型
    /// </summary>
    public enum PunchDirection { Left, Right, Up }

    /// <summary>
    /// 現在パンチ中かどうかを示すプロパティ
    /// </summary>
    public bool IsPunching { get; private set; }

    /// <summary>
    /// 直前のパンチ方向
    /// </summary>
    public PunchDirection LastPunchDirection { get; private set; }

    private void Awake()
    {
        // 初期状態では全てのヒットボックスを無効化しておく
        SetHitboxActive(m_leftHitbox,  false);
        SetHitboxActive(m_rightHitbox, false);
        SetHitboxActive(m_upHitbox,    false);
    }

    /// <summary>
    /// パンチを実行するメソッド。
    /// 最寄りの敵を探し方向を判定してヒットボックスを起動する。
    /// </summary>
    public void ExecutePunch()
    {
        // 既にパンチ中は重複実行しない
        if (IsPunching) return;

        // シーン内の全EnemyBaseを検索
        EnemyBase[] enemies = FindObjectsByType<EnemyBase>(FindObjectsSortMode.None);
        if (enemies.Length == 0)
        {
            Debug.Log("敵が存在しないためパンチをスキップ");
            return;
        }

        // 最も近い敵を選択する
        EnemyBase nearest = null;
        float minDist = float.MaxValue;
        foreach (EnemyBase enemy in enemies)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            // 距離が現在の最小値より小さい場合に更新
            if (dist < minDist)
            {
                minDist   = dist;
                nearest   = enemy;
            }
        }

        if (nearest == null) return;

        // 敵との差分ベクトルから方向を判定
        Vector2 diff = nearest.transform.position - transform.position;
        PunchDirection direction = DetermineDirection(diff);

        Debug.Log($"パンチ方向: {direction}（最寄り敵: {nearest.name}）");
        StartCoroutine(ActivateHitbox(direction));
    }

    /// <summary>
    /// プレイヤーから見た敵の相対位置をもとにパンチ方向（左・右・上）を決定する。
    /// </summary>
    private PunchDirection DetermineDirection(Vector2 diff)
    {
        // Y成分がX成分の絶対値より大きければ「上」
        if (diff.y > Mathf.Abs(diff.x))
        {
            return PunchDirection.Up;
        }

        // X成分の正負で「右」か「左」を決定
        return diff.x >= 0 ? PunchDirection.Right : PunchDirection.Left;
    }

    /// <summary>
    /// 指定したヒットボックスを一時的に有効化するコルーチン。
    /// IsPunching フラグとLastPunchDirectionをスプライト管理用に更新する。
    /// </summary>
    private IEnumerator ActivateHitbox(PunchDirection direction)
    {
        // パンチ開始：スプライト管理用のプロパティを更新
        LastPunchDirection = direction;
        IsPunching = true;
        AudioManager.Instance.PlaySE(SEName.PUNCH_SE_NAME);

        // 方向に対応したヒットボックスを選択
        GameObject hitbox = direction switch
        {
            PunchDirection.Left  => m_leftHitbox,
            PunchDirection.Right => m_rightHitbox,
            PunchDirection.Up    => m_upHitbox,
            _                    => null
        };

        if (hitbox == null)
        {
            Debug.LogWarning($"方向 {direction} に対応するヒットボックスが未設定です");
            IsPunching = false;
            yield break;
        }

        // ヒットボックスを有効化し、指定時間後に無効化
        SetHitboxActive(hitbox, true);
        yield return new WaitForSeconds(m_hitboxDuration);
        SetHitboxActive(hitbox, false);

        // パンチ終了
        IsPunching = false;
    }

    /// <summary>
    /// ヒットボックスオブジェクトのアクティブ状態を安全に切り替えるヘルパー
    /// </summary>
    private void SetHitboxActive(GameObject hitbox, bool active)
    {
        if (hitbox != null)
        {
            hitbox.SetActive(active);
        }
    }
}
