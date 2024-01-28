using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    private static readonly int tint = Shader.PropertyToID("_Tint");
    private static readonly int hitFlash = Shader.PropertyToID("_HitFlash");
    
    private const float StunDuration = 0.5f;
    private const float HitFlashDuration = 0.1f;
    
    public event Action<int> OnPlayerScoreChanged;
    
    public Color Color { get; private set; }
    public int Score { get; private set; }

    [SerializeField] private int scorePerSecond = 100;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private PlayerInput playerInput;
    public PlayerInput PlayerInput
    {
        get
        {
            if (!playerInput)
            {
                playerInput = GetComponent<PlayerInput>();
            }
            
            return playerInput;
        }
    }
    
    public Activity KingPreferredActivity { get; private set; } = Activity.Idle;
    
    private readonly WaitForSeconds hitFlashDuration = new(HitFlashDuration);
    private readonly WaitForSeconds stunDuration = new(StunDuration - HitFlashDuration);
    private readonly WaitForSeconds poseInvulnerabilityDuration = new(0.2f);

    private bool isInvulnerable;
    
    private void OnEnable()
    {
        King.AddPlayer(this);
        King.OnPreferredActivityChanged += OnPreferredActivityChanged;
    }

    private void OnDisable()
    {
        King.RemovePlayer(this);
        King.OnPreferredActivityChanged -= OnPreferredActivityChanged;
    }
    
    private void OnPreferredActivityChanged(Activity _activity)
    {
        KingPreferredActivity = _activity;
    }

    public void SetColor(Color _color)
    {
        Color = _color;
        spriteRenderer.material.SetColor(tint, _color);
    }
    
    public void AddScore()
    {
        Score += scorePerSecond;
        OnPlayerScoreChanged?.Invoke(scorePerSecond);
    }

    public bool TryHit()
    {
        if (isInvulnerable)
            return false;
        
        StopCoroutine(Stun());
        StartCoroutine(Stun());

        return true;
    }

    public void TriggerInvulnerability()
    {
        StopCoroutine(PerformInvulnerability());
        StartCoroutine(PerformInvulnerability());
    }
    
    private IEnumerator PerformInvulnerability()
    {
        isInvulnerable = true;
        
        yield return poseInvulnerabilityDuration;
        
        isInvulnerable = false;
    }

    private IEnumerator Stun()
    {
        PlayerInput.DeactivateInput();

        yield return HitFlash();

        yield return stunDuration;
        
        PlayerInput.ActivateInput();
    }
    
    private IEnumerator HitFlash()
    {
        spriteRenderer.material.SetFloat(hitFlash, 1f);
        
        yield return hitFlashDuration;
        
        spriteRenderer.material.SetFloat(hitFlash, 0f);
    }
}
