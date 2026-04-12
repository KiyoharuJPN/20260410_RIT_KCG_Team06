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
    // 敵のHP
    [SerializeField,Tooltip("敵の初期HP")]
    protected float hp = 1;
    // 敵のID
    [SerializeField,Tooltip("ID")]
    protected int enemyID = 0;


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
            // ダメージ処理
            hp -= 1f; // 仮のダメージ値
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

        // 只有不等于0才更新方向
        if (dirX != 0)
        {
            FacingRight = dirX > 0;
        }

        animator.SetBool("FacingRight", FacingRight);
    }
}
