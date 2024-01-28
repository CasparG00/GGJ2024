using System.Collections.Generic;
using UnityEngine;

public class JoinMessageHandler : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    
    private HashSet<Player> playerList = new();
    
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
    
    private void OnPlayerConnected(Player _player)
    {
        playerList.Add(_player);
        
        UpdateUI();
    }
    
    private void OnPlayerDisconnected(Player _player)
    {
        playerList.Remove(_player);
        
        UpdateUI();
    }

    private void UpdateUI()
    {
        content.gameObject.SetActive(playerList.Count <= 0);
    }
}
