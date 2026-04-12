using UnityEngine;

public class EnemyRepetition : EnemyBase
{
    // 移動の基準点
    protected Vector2 movePivot = Vector2.zero;
    protected Vector2 prevPosition = Vector2.zero;

    protected override void Start()
    {
        base.Start();
        movePivot = transform.localPosition;
    }

    virtual protected void CalcEnemyMovement()
    {

    }
}
