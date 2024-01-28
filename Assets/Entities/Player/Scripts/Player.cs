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
    
    public PlayerInput PlayerInput { get; private set; }
    
    private readonly WaitForSeconds hitFlashDuration = new(HitFlashDuration);
    private readonly WaitForSeconds stunDuration = new(StunDuration - HitFlashDuration);
    
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

    public void Hit()
    {
        StopCoroutine(Stun());
        StartCoroutine(Stun());
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
