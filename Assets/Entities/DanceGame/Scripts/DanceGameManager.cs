using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DanceGameManager : MonoBehaviour
{
    [Tooltip("Every unique dance pose should be added to this array.")]
    [SerializeField] private DanceMove[] danceMoves;
    
    [SerializeField] private DanceGame danceMovePrefab;
    [SerializeField] private int danceMoveSetSize = 10;
    
    private List<DanceMove> danceMoveSet;
    private readonly Dictionary<PlayerInput, DanceGame> activeDanceGames = new();

    private void OnEnable()
    {
        CreateDanceMoveList();
    }
    
    private void OnDisable()
    {
        foreach (var element in activeDanceGames)
        {
            element.Value.EndDanceGame();
        }
        
        activeDanceGames.Clear();
    }

    public void OnPlayerJoined(PlayerInput _player)
    {
        DanceGame danceGame = Instantiate(danceMovePrefab, transform);
        danceGame.StartDanceGame(_player, danceMoveSet);
            
        activeDanceGames[_player] = danceGame;
    }
    
    public void OnPlayerLeft(PlayerInput _player)
    {
        activeDanceGames[_player].EndDanceGame();
        activeDanceGames.Remove(_player);
    }

    private void CreateDanceMoveList()
    {
        danceMoveSet = new List<DanceMove>();
        
        // Create a random set of dance moves.
        for (var i = 0; i < danceMoveSetSize; i++)
        {
            danceMoveSet.Add(danceMoves[Random.Range(0, danceMoves.Length)]);
        }

        // Update all active dance games.
        foreach (var element in activeDanceGames)
        {
            // If a player has left the dance game will be null.
            // In this case a new one needs to be created.
            if (element.Value == null)
            {
                DanceGame danceGame = Instantiate(danceMovePrefab, transform);
                danceGame.StartDanceGame(element.Key, danceMoveSet);
                
                activeDanceGames[element.Key] = danceGame;
            }
            
            // Reinitialize the dance game.
            activeDanceGames[element.Key].StartDanceGame(element.Key, danceMoveSet);
        }
    }
}
