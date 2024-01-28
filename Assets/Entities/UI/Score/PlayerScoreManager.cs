using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreManager : MonoBehaviour
{
    [SerializeField] private PlayerScoreCell playerScoreCellPrefab;
    
    public List<Player> players = new();
    
    private List<PlayerScoreCell> playerScoreCells = new();

    private void OnEnable()
    {
        King.OnHumorLimitReached += OnHumorLimitReached;
        
        PlayerAssigner.OnPlayerConnected += OnPlayerConnected;
        PlayerAssigner.OnPlayerDisconnected += OnPlayerDisconnected;
    }
    
    private void OnDisable()
    {
        King.OnHumorLimitReached -= OnHumorLimitReached;
        
        PlayerAssigner.OnPlayerConnected -= OnPlayerConnected;
        PlayerAssigner.OnPlayerDisconnected -= OnPlayerDisconnected;
    }
    
    private void OnHumorLimitReached(bool _win)
    {
        foreach (var playerScoreCell in playerScoreCells)
        {
            Destroy(playerScoreCell.gameObject);
        }
    }

    private void ResetCells()
    {
        foreach (var playerScoreCell in playerScoreCells)
        {
            Destroy(playerScoreCell.gameObject);
        }
        
        playerScoreCells.Clear();
        
        for (int i = 0; i < players.Count; i++)
        {
            PlayerScoreCell playerScoreCell = Instantiate(playerScoreCellPrefab, transform);
            playerScoreCell.Refresh(players[i].Color, i, players[i].Score);
            playerScoreCells.Add(playerScoreCell);
        }
    }

    private void RefreshCells(int _scoreChange)
    {
        for (int i = 0; i < players.Count; i++)
        {
            playerScoreCells[i].Refresh(players[i].Color, i, players[i].Score);
        }
    }
    
    private void OnPlayerConnected(Player _player)
    {
        players.Add(_player);
        
        _player.OnPlayerScoreChanged += RefreshCells;
        
        ResetCells();
    }
    
    private void OnPlayerDisconnected(Player _player)
    {
        players.Remove(_player);
        
        _player.OnPlayerScoreChanged -= RefreshCells;
        
        ResetCells();
    }
}
