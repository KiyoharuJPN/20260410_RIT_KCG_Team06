using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// プレイヤーの残機を管理するクラス。
/// </summary>
public class PlayerLives : MonoBehaviour
{
    [SerializeField]
    [Tooltip("初期残機数")]
    private int m_initialLives = 3;

    [SerializeField]
    [Tooltip("被弾後の無敵時間（秒）")]
    private float m_invincibleDuration = 1.5f;

    /// <summary>
    /// 現在の残機数
    /// </summary>
    public int CurrentLives { get; private set; }

    /// <summary>
    /// 現在無敵状態かどうか
    /// </summary>
    public bool IsInvincible { get; private set; }

    /// <summary>
    /// 残機が変化したときに発火するイベント（UI更新などに使用）
    /// </summary>
    public UnityEvent<int> OnLivesChanged;

    /// <summary>
    /// 残機が0になったときに発火するイベント（ゲームオーバー通知用）
    /// </summary>
    public UnityEvent OnGameOver;

    private void Start()
    {
        // 残機を初期値で設定し、初回UI更新を通知
        CurrentLives = m_initialLives;
        OnLivesChanged?.Invoke(CurrentLives);
    }

    /// <summary>
    /// 敵のトリガーに接触したときの処理
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 無敵中はダメージを受けない
        if (IsInvincible) return;

        // 接触したオブジェクトが敵かどうかをコンポーネントで判定
        if (collision.GetComponent<EnemyBase>() != null)
        {
            TakeDamage(1);
        }
    }

    /// <summary>
    /// プレイヤーがダメージを受けるメソッド。
    /// 残機を amount 分減らし、0以下になったらゲームオーバーを通知する。
    /// </summary>
    public void TakeDamage(int amount)
    {
        // 残機を減算（0未満にはしない）
        CurrentLives = Mathf.Max(0, CurrentLives - amount);
        OnLivesChanged?.Invoke(CurrentLives);

        Debug.Log($"被弾しました：残機 {CurrentLives + amount} -> {CurrentLives}");

        // 残機が0になったらゲームオーバーを通知
        if (CurrentLives <= 0)
        {
            Debug.Log("残機0：ゲームオーバーを通知します");
            OnGameOver?.Invoke();
            return;
        }

        // 残機が残っている場合は無敵時間を開始
        StartCoroutine(InvincibilityCoroutine());
    }

    /// <summary>
    /// 残機を回復するメソッド（外部から呼び出し可能）
    /// </summary>
    public void AddLives(int amount)
    {
        CurrentLives += amount;
        OnLivesChanged?.Invoke(CurrentLives);
        Debug.Log($"残機回復：現在 {CurrentLives}");
    }

    /// <summary>
    /// 被弾後、一定時間無敵状態にするコルーチン
    /// </summary>
    private IEnumerator InvincibilityCoroutine()
    {
        IsInvincible = true;
        Debug.Log($"無敵開始（{m_invincibleDuration}秒）");

        yield return new WaitForSeconds(m_invincibleDuration);

        IsInvincible = false;
        Debug.Log("無敵終了");
    }
}
