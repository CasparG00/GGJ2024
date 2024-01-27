using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DanceGame : MonoBehaviour
{
    // SpriteRenderer should probably change to an actual dance move cell class.
    [SerializeField] private RectTransform danceMoveHintContainer;
    [SerializeField] private DanceMoveHint danceMoveHintPrefab;
    [SerializeField] private int maximumDanceMoveHints = 5;
    
    private PlayerInput owner;
    private List<DanceMove> danceMoveSet;
    
    private int danceMoveIndex;
    
    // The key is the id of the dance move.
    // The value is the action that triggers the dance move.
    private Dictionary<int, InputAction> danceMoveActions = new();
    
    private Queue<DanceMoveHint> danceMoveHints = new();
    
    private readonly WaitForSeconds stunDuration = new(0.5f);
    private bool isStunned;

    private void Update()
    {
        if (danceMoveActions.Count == 0)
            return;

        if (isStunned)
            return;
        
        foreach (var element in danceMoveActions)
        {
            if (element.Value.triggered)
            {
                OnDanceMovePressed(element.Key);
            }
        }
    }
    
    public void StartDanceGame(PlayerInput _owner, List<DanceMove> _set)
    {
        owner = _owner;
        danceMoveSet = _set;
        danceMoveIndex = 0;
        
        SetDanceMoveActions();

        // Create a bunch of dance move hints.
        for (int i = 0; i < maximumDanceMoveHints; i++)
        {
            DanceMoveHint danceMoveObject = Instantiate(danceMoveHintPrefab, danceMoveHintContainer);
            danceMoveObject.SetSprite(danceMoveSet[i % danceMoveSet.Count].sprite);
            danceMoveHints.Enqueue(danceMoveObject);
        }
    }
    
    public void EndDanceGame()
    {
        Destroy(gameObject);
    }
    
    private void SetDanceMoveActions()
    {
        // Reset the dictionary and add all dance move actions.
        danceMoveActions ??= new Dictionary<int, InputAction>();
        danceMoveActions.Clear();

        danceMoveActions[1] = owner.actions.FindAction("Dance 1");
        danceMoveActions[2] = owner.actions.FindAction("Dance 2");
        danceMoveActions[3] = owner.actions.FindAction("Dance 3");
    }
    
    private void OnDanceMovePressed(int _id)
    {
        bool pressedCorrectDanceMove = _id == danceMoveSet[danceMoveIndex].id;
        if (pressedCorrectDanceMove)
        {
            // The first dance move hint in the queue is always the one we just pressed.
            DanceMoveHint danceMoveObject = danceMoveHints.Dequeue();
            danceMoveObject.TriggerDespawn();
            
            // Spawn a new dance move hint and put it in the back of the queue.
            int offsetIndex = (danceMoveIndex + maximumDanceMoveHints) % danceMoveSet.Count;
            danceMoveObject = Instantiate(danceMoveHintPrefab, danceMoveHintContainer);
            danceMoveObject.SetSprite(danceMoveSet[offsetIndex].sprite);
            
            danceMoveHints.Enqueue(danceMoveObject);
            
            danceMoveIndex = (danceMoveIndex + 1) % danceMoveSet.Count;
        }
        else
        {
            StopCoroutine(Stun());
            StartCoroutine(Stun());
        }
    }

    private IEnumerator Stun()
    {
        isStunned = true;
        
        yield return stunDuration;
        
        isStunned = false;
    }
}