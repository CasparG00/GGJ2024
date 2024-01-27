using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAssigner : MonoBehaviour
{
    public static event System.Action<Player> OnPlayerConnected;
    public static event System.Action<Player> OnPlayerDisconnected;
    
    [SerializeField] private Color[] playerColors;
    
    public void OnPlayerJoined(PlayerInput _playerInput)
    {
        Player player = _playerInput.GetComponent<Player>();
        player.SetColor(playerColors[_playerInput.playerIndex]);
        
        OnPlayerConnected?.Invoke(player);
    }

    public void OnPlayerLeft(PlayerInput _playerInput)
    {
        Player player = _playerInput.GetComponent<Player>();
        
        OnPlayerDisconnected?.Invoke(player);
    }
}
