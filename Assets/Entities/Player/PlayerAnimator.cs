using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private new Rigidbody2D rigidbody;
    
    private static readonly int moveSpeed = Animator.StringToHash("MoveSpeed");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponentInParent<Rigidbody2D>();
    }

    private void LateUpdate()
    {
        float velocityX = rigidbody.velocity.x;
        if (velocityX != 0)
        {
            spriteRenderer.flipX = Mathf.Sign(velocityX) > 0;
        }
        
        animator.SetFloat(moveSpeed, Mathf.Abs(velocityX));
    }
}
