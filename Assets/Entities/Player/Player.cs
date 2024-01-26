using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public event Action<int> OnPlayerScoreChanged;
    public int Score { get; private set; }

    [SerializeField] private int scorePerSecond = 100;
    
    private Activity kingPreferredActivity;
    // TODO: Remove default value.
    private Activity performingActivity = Activity.Fight;
    
    private void OnEnable()
    {
        King.OnPreferredActivityChanged += OnPreferredActivityChanged;
    }

    private void OnDisable()
    {
        King.OnPreferredActivityChanged -= OnPreferredActivityChanged;
    }

    private void Update()
    {
        // TODO: Change this check to actually check the action.
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (kingPreferredActivity == performingActivity)
            {
                Score += scorePerSecond;
                OnPlayerScoreChanged?.Invoke(scorePerSecond);
            }
        }
    }

    private void OnPreferredActivityChanged(Activity _preferred)
    {
        kingPreferredActivity = _preferred;
    }
}
