using System.Collections;
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
    
    private Vector2 punchDirection = Vector2.right;
    private readonly RaycastHit2D[] punchResults = new RaycastHit2D[5];
    
    private readonly WaitForSeconds punchDelay = new(0.3f);
    private bool currentlyPunching;

    private void Awake()
    {
        player = GetComponent<Player>();
        playerInput = GetComponent<PlayerInput>();
        rigidbody = GetComponent<Rigidbody2D>();

        moveAction = playerInput.actions.FindAction("Move");
        punchAction = playerInput.actions.FindAction("Punch");

        // Reset the dictionary and add all dance move actions.
        danceMoveActions ??= new Dictionary<int, InputAction>();
        danceMoveActions.Clear();

        danceMoveActions[1] = playerInput.actions.FindAction("Dance 1");
        danceMoveActions[2] = playerInput.actions.FindAction("Dance 2");
        danceMoveActions[3] = playerInput.actions.FindAction("Dance 3");
    }

    private void FixedUpdate()
    {
        float input = moveAction.ReadValue<Vector2>().x;
        rigidbody.velocity = Vector2.right * (input * moveSpeed);
    }

    private void Update()
    {
        if (punchAction.triggered && !currentlyPunching)
        {
            StartCoroutine(PerformPunch());
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

    private IEnumerator PerformPunch()
    {
        currentlyPunching = true;

        if (Mathf.Abs(rigidbody.velocity.x) > 0.1f)
        {
            punchDirection = rigidbody.velocity.x >= 0 ? Vector2.right : Vector2.left;
        }
        
        playerInput.DeactivateInput();
        
        yield return punchDelay;
        
        int size = Physics2D.RaycastNonAlloc(rigidbody.worldCenterOfMass, punchDirection, punchResults, punchDistance, playerLayer);
        if (size == 0)
            yield break;

        for (int i = 0; i < size; i++)
        {
            if (punchResults[i].transform.TryGetComponent(out Player otherPlayer))
            {
                if (otherPlayer == player)
                    continue;
                
                player.AddScore();
                otherPlayer.Hit();
            }
        }

        playerAnimator.TriggerPunch(System.Math.Sign(punchDirection.x));
        
        playerInput.ActivateInput();
        
        currentlyPunching = false;
    }
}
