using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction moveAction;

    // Unity has a depricated variable built in for rigidbodies, so we need to hide that.
    private new Rigidbody2D rigidbody;

    [SerializeField] private float moveSpeed = 5f;
    
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        moveAction = playerInput.actions.FindAction("Move");
    }

    private void FixedUpdate()
    {
        float input = moveAction.ReadValue<float>();
        rigidbody.velocity = Vector2.right * (input * moveSpeed);
    }
}
