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
    
    private Player owner;
    private List<DanceMove> danceMoveSet;
    
    private int danceMoveIndex;
    
    // The key is the id of the dance move.
    // The value is the action that triggers the dance move.
    private Dictionary<int, InputAction> danceMoveActions = new();
    
    private Queue<DanceMoveHint> danceMoveHintsQueue = new();

    private void Update()
    {
        if (danceMoveActions.Count == 0)
            return;
        
        foreach (var element in danceMoveActions)
        {
            if (element.Value.triggered)
            {
                OnDanceMovePressed(element.Key);
            }
        }
    }
    
    public void StartDanceGame(Player _owner, List<DanceMove> _set)
    {
        owner = _owner;
        danceMoveSet = _set;
        danceMoveIndex = 0;
        
        SetDanceMoveActions();

        foreach (var hint in danceMoveHintsQueue)
        {
            Destroy(hint.gameObject);
        }
        
        // Create a bunch of dance move hints.
        for (int i = 0; i < maximumDanceMoveHints; i++)
        {
            DanceMoveHint danceMoveObject = Instantiate(danceMoveHintPrefab, danceMoveHintContainer);
            danceMoveObject.SetDanceMoveSprites(danceMoveSet[i % danceMoveSet.Count], _owner);
            

            danceMoveHintsQueue.Enqueue(danceMoveObject);
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

        danceMoveActions[1] = owner.PlayerInput.actions.FindAction("Dance 1");
        danceMoveActions[2] = owner.PlayerInput.actions.FindAction("Dance 2");
        danceMoveActions[3] = owner.PlayerInput.actions.FindAction("Dance 3");
    }
    
    private void OnDanceMovePressed(int _id)
    {
        if (_id == danceMoveSet[danceMoveIndex].id)
        {
            // The first dance move hint in the queue is always the one we just pressed.
            DanceMoveHint danceMoveObject = danceMoveHintsQueue.Dequeue();
            danceMoveObject.TriggerDespawn();
            
            // Spawn a new dance move hint and put it in the back of the queue.
            int offsetIndex = (danceMoveIndex + maximumDanceMoveHints) % danceMoveSet.Count;
            danceMoveObject = Instantiate(danceMoveHintPrefab, danceMoveHintContainer);
            danceMoveObject.SetDanceMoveSprites(danceMoveSet[offsetIndex], owner);

            danceMoveHintsQueue.Enqueue(danceMoveObject);
            
            owner.AddScore();
            
            danceMoveIndex = (danceMoveIndex + 1) % danceMoveSet.Count;
        }
        else
        {
            owner.Hit();
        }
    }
}