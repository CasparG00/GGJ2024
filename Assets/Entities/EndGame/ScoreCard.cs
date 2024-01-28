using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image playerImage;
    [SerializeField] private Sprite winSprite;
    [SerializeField] private Sprite loseSprite;
    
    private static readonly int tint = Shader.PropertyToID("_Tint");

    public void Refresh(Player _player, bool _win)
    {
        string text = $"Player {_player.PlayerInput.playerIndex + 1}";
        text += _win ? " Wins!" : " Loses!";
        
        playerText.text = text;
        playerText.color = _player.Color;
        
        scoreText.text = $"Score: {_player.Score.ToString()}";
        scoreText.color = _player.Color;
        
        playerImage.sprite = _win ? winSprite : loseSprite;
        playerImage.material.SetColor(tint, _player.Color);
    }
}
