using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CloudFloor : MonoBehaviour
{
    [SerializeField,Tooltip("プレイヤーが接触した際の重力スケール")] 
    float gravityScale = 0.01f;
    [SerializeField, Tooltip("プレイヤーが落下する際の最大速度")]
    float maxFallSpeed = -1f; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb.linearVelocityY < 0)
            {
                playerRb.linearVelocityY = 0;
                playerRb.gravityScale = gravityScale;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb.linearVelocityY < maxFallSpeed)
            {
                playerRb.linearVelocityY = maxFallSpeed;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb.linearVelocityY > 0)
            {
                playerRb.gravityScale = StageManager.Instance.MasterData.JumpGravityScale; // Reset to JumpGravityScale when the player is moving upwards
            }
            else if (playerRb.linearVelocityY <= 0)
            {
                playerRb.gravityScale = StageManager.Instance.MasterData.FallGravityScale; // Reset to FallGravityScale when the player is moving downwards
            }
            else
            {
                playerRb.gravityScale = 1.0f; // Default gravity scale if the player is stationary
            }
        }
    }
}