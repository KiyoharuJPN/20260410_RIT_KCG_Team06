using Unity.VisualScripting;
using UnityEngine;

public class EnemyVMove : EnemyRepetition
{
    private void FixedUpdate()
    {
        CalcEnemyMovement();
    }

    override protected void CalcEnemyMovement()
    {
        float y = Mathf.Sin(Time.time * moveSpeed) * distance;
        transform.localPosition = movePivot + new Vector2(0, y);
    }
}
