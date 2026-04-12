using UnityEngine;

public class EnemyHMove : EnemyRepetition
{
    private void FixedUpdate()
    {
        prevPosition = transform.position;
        CalcEnemyMovement();
        SetMoveDirection();
    }

    override protected void CalcEnemyMovement()
    {
        float x = Mathf.Sin(Time.time * moveSpeed) * distance;
        transform.localPosition = movePivot + new Vector2(x, 0);
    }

    override protected void SetMoveDirection()
    {
        float dirX = transform.localPosition.x - prevPosition.x;

        if (dirX != 0)
        {
            FacingRight = dirX > 0;
        }

        // アニメーションの反転
        animator.SetBool("FacingRight", FacingRight);
    }
}
