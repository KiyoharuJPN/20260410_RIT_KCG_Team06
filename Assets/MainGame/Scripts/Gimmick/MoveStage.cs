using UnityEngine;

public class MoveStage : MonoBehaviour
{
    [Tooltip("移動の速さ")]
    public float moveSpeed = 2f;
    [Tooltip("移動の距離")]
    public float distance = 2;
    [Tooltip("移動の方向")]
    public bool isLeft = false;
    // 移動の基準点
    protected Vector2 movePivot = Vector2.zero;
    // Frameごとの移動量を計算するための変数
    Vector2 movedDisplacement = Vector2.zero;
    // プレイヤーが移動する際の微調整用の係数
    float deviation = 1.02f;

    protected virtual void Start()
    {
        // 基準点を初期化
        movePivot = transform.localPosition;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // プレイヤーが接触している場合、プレイヤーを移動させる
        if (collision.gameObject.CompareTag("Player"))
        {
            // プレイヤーを移動させる
            if (collision.gameObject.TryGetComponent(out Rigidbody2D rb))
            {
                // プレイヤーがジャンプしていない且つプレイヤーがプラットフォームに乗っている場合のみ移動させる
                if (rb.linearVelocityY == 0 && gameObject.transform.position.y+1.0f < collision.gameObject.transform.position.y)
                {
                    rb.MovePosition(rb.position + (movedDisplacement * deviation));
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 移動処理
        Vector2 targetPosition = movePivot + new Vector2((Mathf.PingPong(Time.time * moveSpeed, distance) * (isLeft?-1:1)), 0f); 
        movedDisplacement = targetPosition - (Vector2)transform.localPosition;
        transform.localPosition = targetPosition;

    }
}
