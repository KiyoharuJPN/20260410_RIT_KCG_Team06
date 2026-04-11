using UnityEngine;

public class EnemyHMove : EnemyRepetition
{
    private void FixedUpdate()
    {
        float x = Mathf.Sin(Time.time * moveSpeed) * distance;
        transform.localPosition = movePivot + new Vector2(x, 0);
    }
}
