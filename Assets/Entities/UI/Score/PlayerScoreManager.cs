using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScoreManager : MonoBehaviour
{
    [SerializeField] private PlayerScoreCell playerScoreCellPrefab;
    
    private List<Player> players = new();
    
    private List<PlayerScoreCell> playerScoreCells = new();

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
    
    public void OnPlayerJoined(PlayerInput _playerInput)
    {
        Player player = _playerInput.GetComponent<Player>();
        players.Add(player);
        
        player.OnPlayerScoreChanged += RefreshCells;
        
        ResetCells();
    }
    
    public void OnPlayerLeft(PlayerInput _playerInput)
    {
        Player player = _playerInput.GetComponent<Player>();
        players.Remove(player);
        
        player.OnPlayerScoreChanged -= RefreshCells;
        
        ResetCells();
    }
}
