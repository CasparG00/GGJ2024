using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private ScoreCard scoreCardPrefab;
    [SerializeField] private RectTransform scoreCardContainer;

    private List<Player> players = new();
    
    private void OnEnable()
    {
        PlayerAssigner.OnPlayerConnected += OnPlayerConnected;
        PlayerAssigner.OnPlayerDisconnected += OnPlayerDisconnected;
        
        King.OnHumorLimitReached += OnHumorLimitReached;
    }

    private void OnDisable()
    {
        PlayerAssigner.OnPlayerConnected -= OnPlayerConnected;
        PlayerAssigner.OnPlayerDisconnected -= OnPlayerDisconnected;
        
        King.OnHumorLimitReached -= OnHumorLimitReached;
    }
    
    private void OnPlayerConnected(Player _player)
    {
        players.Add(_player);
    }
    
    private void OnPlayerDisconnected(Player _player)
    {
        players.Remove(_player);
    }

    private void OnHumorLimitReached(bool _win)
    {
        ShowGameOver(_win);
    }

    private void ShowGameOver(bool _win)
    {
        titleText.text = _win ? "The king is satisfied!" : "The king is NOT satisfied.";
        
        foreach (var player in players)
        {
            ScoreCard scoreCard = Instantiate(scoreCardPrefab, scoreCardContainer);
            scoreCard.Refresh(player, _win);
        }
        
        content.gameObject.SetActive(true);
    }
    
    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
