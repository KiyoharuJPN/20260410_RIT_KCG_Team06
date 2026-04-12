using UnityEngine;

public class EnemyIdle : EnemyBase
{
    private void FixedUpdate()
    {
        SetMoveDirection();
    }
}
