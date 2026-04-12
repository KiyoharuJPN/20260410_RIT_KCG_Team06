using UnityEngine;

public class EnemyRepetition : EnemyBase
{
    [Tooltip("移動の速さ")]
    public float moveSpeed = 2f;
    [Tooltip("移動の距離")]
    public float distance = 2;
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
