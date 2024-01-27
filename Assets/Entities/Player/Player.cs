using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    public event Action<int> OnPlayerScoreChanged;
    public int Score { get; private set; }

    [SerializeField] private int scorePerSecond = 100;
    
    public PlayerInput PlayerInput { get; private set; }
    
    private Activity kingPreferredActivity;

    private readonly WaitForSeconds stunDuration = new(0.5f);
    private bool isStunned;

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

    public void ExecuteStun()
    {
        StopCoroutine(Stun());
        StartCoroutine(Stun());
        Debug.Log("Hit");
    }

    private IEnumerator Stun()
    {
        isStunned = true;

        yield return stunDuration;

        isStunned = false;
    }
}
