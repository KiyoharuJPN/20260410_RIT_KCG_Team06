using UnityEngine;

public class EnemyA : EnemyBase
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
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

}
