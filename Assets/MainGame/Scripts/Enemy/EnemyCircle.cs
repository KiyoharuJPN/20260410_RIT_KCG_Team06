using UnityEngine;
using UnityEngine.UIElements;

public class EnemyCircle : EnemyRepetition
{
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        movePivot = transform.position + new Vector3(distance, 0, 0);
    }

    private void FixedUpdate()
    {
        prevPosition = transform.position;
        CalcEnemyMovement();
        SetMoveDirection();
    }

    override protected void CalcEnemyMovement()
    {
        float x = Mathf.Cos(Time.time * moveSpeed) * distance;
        float y = Mathf.Sin(Time.time * moveSpeed) * distance;

        transform.position = movePivot + new Vector2(x, y);
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
