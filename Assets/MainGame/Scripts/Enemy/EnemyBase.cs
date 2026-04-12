using UnityEngine;

//[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]

public class EnemyBase : MonoBehaviour
{
    // 敵のアニメータ
    protected Animator animator;
    // 敵の移動方向
    protected bool FacingRight = true;
    [SerializeField,Tooltip("攻撃されるときのエフェクト")]
    private GameObject hitEffect;
    // 敵のID
    [SerializeField,Tooltip("ID")]
    protected int enemyID = 0;
    // 敵のHP
    [SerializeField, Tooltip("敵の初期HP")]
    protected float hp = 1;
    public void SetHP(float hp) => this.hp = hp;

    // 移動する敵用のパラメータ
    [Tooltip("移動の速さ")]
    protected float moveSpeed = 2f;
    public void SetMoveSpeed(float speed) => moveSpeed = speed;
    [Tooltip("移動の距離")]
    protected float distance = 2;
    public void SetDistance(float dist) => distance = dist;


    // 初期化
    virtual protected void Start()
    {
        animator = GetComponent<Animator>();
    }

    // HP処理
    virtual protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttack"))
        {
            // エフェクトの生成
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }

            // ダメージ処理
            hp -= 1f; // 仮のダメージ値

            if (FeverGaugeManager.Instance.IsFever) hp = 0; // フィーバー中は即死

            if (hp <= 0)
            {
                // 敵が倒されたときの処理
                ResultManager.Instance.AddKill(); // キルカウントを増やす
                Destroy(gameObject); // 敵オブジェクトを破壊
            }
        }
    }

    // モンスターが向かう方向の設定
    virtual protected void SetMoveDirection()
    {
        // プレイヤーの位置を取得
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        float dirX = player.transform.position.x - transform.position.x;

        // ゼロを無視する
        if (dirX != 0)
        {
            FacingRight = dirX > 0;
        }

        animator.SetBool("FacingRight", FacingRight);
    }
}
