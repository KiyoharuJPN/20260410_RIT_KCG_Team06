using System.Collections;
using UnityEngine;

/// <summary>
/// プレイヤーの空中パンチ攻撃を管理するクラス。
/// 最寄りの敵の方向（左・右・上）を判定し、対応するヒットボックスを一時的に有効化する。
/// </summary>
/// <remarks>
/// セットアップ方法:
/// 1. プレイヤーの子オブジェクトとして「PunchHitbox_Left」「PunchHitbox_Right」「PunchHitbox_Up」を作成する。
/// 2. 各子オブジェクトに BoxCollider2D（IsTrigger=true）をアタッチし、Tag を「Player」に設定する。
/// 3. 各子オブジェクトを本スクリプトの対応フィールドにアタッチする。
/// 4. 各子オブジェクトはデフォルトで非アクティブにしておく。
/// </remarks>
public class PlayerPunchExecutor : MonoBehaviour
{
    [SerializeField, Tooltip("左方向のパンチ判定オブジェクト（Playerタグ・IsTriggerコライダー付き）")]
    private GameObject m_leftHitbox;

    [SerializeField, Tooltip("右方向のパンチ判定オブジェクト（Playerタグ・IsTriggerコライダー付き）")]
    private GameObject m_rightHitbox;

    [SerializeField, Tooltip("上方向のパンチ判定オブジェクト（Playerタグ・IsTriggerコライダー付き）")]
    private GameObject m_upHitbox;

    [SerializeField, Tooltip("パンチ判定の持続時間（秒）")]
    private float m_hitboxDuration = 0.1f;

    /// <summary>
    /// パンチの方向を示す列挙型
    /// </summary>
    private enum PunchDirection { Left, Right, Up }

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
                minDist = dist;
                nearest = enemy;
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
    /// <param name="diff">敵との差分ベクトル</param>
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
    /// 指定したヒットボックスを一時的に有効化するコルーチン
    /// </summary>
    /// <param name="direction">有効化するパンチ方向</param>
    private IEnumerator ActivateHitbox(PunchDirection direction)
    {
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
            yield break;
        }

        // ヒットボックスを有効化
        SetHitboxActive(hitbox, true);

        // 指定時間だけ有効にした後、無効化
        yield return new WaitForSeconds(m_hitboxDuration);
        SetHitboxActive(hitbox, false);
    }

    /// <summary>
    /// ヒットボックスオブジェクトのアクティブ状態を安全に切り替えるヘルパー
    /// </summary>
    /// <param name="hitbox">対象のGameObject</param>
    /// <param name="active">アクティブにするかどうか</param>
    private void SetHitboxActive(GameObject hitbox, bool active)
    {
        if (hitbox != null)
        {
            hitbox.SetActive(active);
        }
    }
}
