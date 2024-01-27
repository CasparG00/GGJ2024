using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Player player;
    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction punchAction;
    
    private Dictionary<int, InputAction> danceMoveActions = new();


    // Unity has a depricated variable built in for rigidbodies, so we need to hide that.
    private new Rigidbody2D rigidbody;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float punchDistance = 2f;
    [SerializeField] private LayerMask playerLayer;
    
    [SerializeField] private PlayerAnimator playerAnimator;

    public bool canMove = true;

    private void Awake()
    {
        player = GetComponent<Player>();
        playerInput = GetComponent<PlayerInput>();
        rigidbody = GetComponent<Rigidbody2D>();

        moveAction = playerInput.actions.FindAction("Move");
        punchAction = playerInput.actions.FindAction("Punch");

        canMove = true;

        // Reset the dictionary and add all dance move actions.
        danceMoveActions ??= new Dictionary<int, InputAction>();
        danceMoveActions.Clear();

        danceMoveActions[1] = playerInput.actions.FindAction("Dance 1");
        danceMoveActions[2] = playerInput.actions.FindAction("Dance 2");
        danceMoveActions[3] = playerInput.actions.FindAction("Dance 3");
    }

    private void FixedUpdate()
    {
        if (!canMove)
            return;
        
        float input = moveAction.ReadValue<Vector2>().x;
        rigidbody.velocity = Vector2.right * (input * moveSpeed);
    }

    private void Update()
    {
        if (!canMove)
            return;

        if (punchAction.triggered)
        {
            CheckPunch();
        }

        if (danceMoveActions.Count != 0)
        {
            foreach (var element in danceMoveActions)
            {
                if (element.Value.triggered)
                {
                    playerAnimator.TriggerPose(element.Key);
                }
            }
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
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(detectionPosition, 1f, playerLayer);

        foreach (Collider2D hitCollider in hitColliders)
        {
            // Do something with the enemy (e.g., attack, damage, etc.)
            Player otherPlayer = hitCollider.GetComponentInParent<Player>();
            
            if (otherPlayer == null || otherPlayer == player)
                continue;
            
            otherPlayer.Hit();

            if (player.KingPreferredActivity is Activity.Fight)
            {
                player.AddScore();
            }
        }
    }
}
