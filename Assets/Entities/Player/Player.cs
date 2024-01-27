using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    public event Action<int> OnPlayerScoreChanged;

    private int score;
    public int Score {
        get { return score; }
        private set
        {
            if (score != value)
            {
                // Execute code after the value has changed
                Debug.Log("Value has changed from " + score + " to " + value);

                // Add your custom logic here


                // Update the backing field
                score = value;
            }
        }
    }

    [SerializeField] private int scorePerSecond = 100;
    
    public PlayerInput PlayerInput { get; private set; }
    
    public Activity KingPreferredActivity { get; private set; }

    private readonly WaitForSeconds stunDuration = new(0.5f);

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();
    }
    
    private void OnEnable()
    {
        King.AddPlayer(this);
    }

    private void OnDisable()
    {
        King.RemovePlayer(this);
    }

    public void AddScore()
    {
        Score += scorePerSecond;
        OnPlayerScoreChanged?.Invoke(scorePerSecond);
    }

    public void Hit()
    {
        StopCoroutine(Stun());
        StartCoroutine(Stun());
    }

    private IEnumerator Stun()
    {
        PlayerInput.DeactivateInput();

        yield return stunDuration;
        
        PlayerInput.ActivateInput();
    }
}
