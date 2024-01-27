using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScoreManager : MonoBehaviour
{
    [SerializeField] private PlayerScoreCell playerScoreCellPrefab;
    
    private List<Player> players = new();
    
    private List<PlayerScoreCell> playerScoreCells = new();

    private void OnEnable()
    {
        PlayerAssigner.OnPlayerConnected += OnPlayerConnected;
        PlayerAssigner.OnPlayerDisconnected += OnPlayerDisconnected;
    }
    
    private void OnDisable()
    {
        PlayerAssigner.OnPlayerConnected -= OnPlayerConnected;
        PlayerAssigner.OnPlayerDisconnected -= OnPlayerDisconnected;
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
            playerScoreCell.Refresh(i, players[i].Score);
            playerScoreCells.Add(playerScoreCell);
        }
    }

    private void RefreshCells(int _scoreChange)
    {
        for (int i = 0; i < players.Count; i++)
        {
            playerScoreCells[i].Refresh(i, players[i].Score);
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
