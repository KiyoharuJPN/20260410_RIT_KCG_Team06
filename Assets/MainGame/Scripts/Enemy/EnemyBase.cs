using UnityEngine;

//[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]

public class EnemyBase : MonoBehaviour
{
    // 敵の潜水艦を発見！
    protected Animator animator;
    // 敵のHP
    [SerializeField,Tooltip("敵の初期HP")]
    protected float hp = 1;

    // アニメータの初期化
    virtual protected void Start()
    {
        animator = GetComponent<Animator>();
    }

    // プレイヤーやプレイヤーの攻撃と衝突したときの処理
    virtual protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // プレイヤーにダメージを与える処理

        }

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


}
