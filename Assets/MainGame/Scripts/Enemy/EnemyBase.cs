using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]

public class EnemyBase : MonoBehaviour
{
    protected Animator animator;
    [SerializeField]
    protected float hp;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
}
