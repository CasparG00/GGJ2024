using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction punchAction;

    // Unity has a depricated variable built in for rigidbodies, so we need to hide that.
    private new Rigidbody2D rigidbody;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float punchDistance = 2f;
    [SerializeField] private LayerMask playerLayer;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rigidbody = GetComponent<Rigidbody2D>();

        moveAction = playerInput.actions.FindAction("Move");
        punchAction = playerInput.actions.FindAction("Punch");

    }

    private void FixedUpdate()
    {
        float input = moveAction.ReadValue<float>();
        rigidbody.velocity = Vector2.right * (input * moveSpeed);
    }

    private void Update()
    {
        if (punchAction.triggered)
        {
            Debug.Log("HA");
            CheckPunch();
        }
    }
    //TODO: Add punch 

    private void CheckPunch()
    {
        // Get the player's position
        Vector2 playerPosition = transform.position;

        Vector2 playerDirection = rigidbody.transform.right;
        // Calculate the position in front of the player based on the direction they are facing
        Vector2 detectionPosition = playerPosition + (playerDirection * punchDistance);

        // Check for enemies in the detection position
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(detectionPosition, 0.1f, playerLayer);

        foreach (Collider2D hitCollider in hitColliders)
        {
            // Do something with the enemy (e.g., attack, damage, etc.)
            Debug.Log("Enemy in front: " + hitCollider.gameObject.name);
        }
    }
}
