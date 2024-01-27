using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private new Rigidbody2D rigidbody;
    
    private static readonly int moveSpeed = Animator.StringToHash("MoveSpeed");
    private static readonly int posing1 = Animator.StringToHash("Posing1");
    private static readonly int posing2 = Animator.StringToHash("Posing2");
    private static readonly int posing3 = Animator.StringToHash("Posing3");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponentInParent<Rigidbody2D>();
    }

    private void LateUpdate()
    {
        animator.SetFloat(moveSpeed, rigidbody.velocity.x);
    }
    
    public void TriggerPose(int _pose)
    {
        int hash = _pose switch
        {
            1 => posing1,
            2 => posing2,
            3 => posing3,
            _ => 0
        };
        
        animator.SetTrigger(hash);
    }
}
