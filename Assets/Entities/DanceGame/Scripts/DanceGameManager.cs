using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DanceGameManager : MonoBehaviour
{
    [Tooltip("Every unique dance pose should be added to this array.")]
    [SerializeField] private DanceMove[] danceMoves;
    
    [SerializeField] private DanceGame danceMovePrefab;
    [SerializeField] private int danceMoveSetSize = 10;
    
    private List<DanceMove> danceMoveSet;
    private readonly Dictionary<Player, DanceGame> activeDanceGames = new();
    
    private Activity kingPreferredActivity = Activity.Idle;

    private void OnEnable()
    {
        King.OnPreferredActivityChanged += OnPreferredActivityChanged;
        King.KingHappy += OnGameOver;
        King.KingAngry += OnGameOver;
        
        PlayerAssigner.OnPlayerConnected += OnPlayerConnected;
        PlayerAssigner.OnPlayerDisconnected += OnPlayerDisconnected;
    }
    
    private void OnDisable()
    {
        King.OnPreferredActivityChanged -= OnPreferredActivityChanged;
        King.KingHappy -= OnGameOver;
        King.KingAngry -= OnGameOver;
        
        PlayerAssigner.OnPlayerConnected -= OnPlayerConnected;
        PlayerAssigner.OnPlayerDisconnected -= OnPlayerDisconnected;
    }

    private void OnGameOver()
    {
        StopAllDanceGames();
    }

    private void OnPreferredActivityChanged(Activity _activity)
    {
        kingPreferredActivity = _activity;
        
        if (kingPreferredActivity is Activity.Dance)
        {
            CreateDanceMoveList();
        }
        else
        {
            StopAllDanceGames();
        }
    }

    private void OnPlayerConnected(Player _player)
    {
        if (kingPreferredActivity is Activity.Dance)
        {
            DanceGame danceGame = Instantiate(danceMovePrefab, transform);
            danceGame.StartDanceGame(_player, danceMoveSet);
            
            activeDanceGames[_player] = danceGame;

            return;
        }
        
        activeDanceGames[_player] = null;
    }
    
    public void OnPlayerDisconnected(Player _player)
    {
        if (kingPreferredActivity is Activity.Dance)
            activeDanceGames[_player].EndDanceGame();

        activeDanceGames.Remove(_player);
    }
    
    private void StopAllDanceGames()
    {
        foreach (var element in activeDanceGames)
        {
            bool isDanceGameActive = element.Value != null;
            if (isDanceGameActive)
            {
                element.Value.EndDanceGame();
            }
        }
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
        List<Player> players = activeDanceGames.Keys.ToList();
        foreach (var player in players)
        {
            // If a player has left the dance game will be null.
            // In this case a new one needs to be created.
            if (activeDanceGames[player] == null)
            {
                DanceGame danceGame = Instantiate(danceMovePrefab, transform);
                activeDanceGames[player] = danceGame;
            }
            
            // Reinitialize the dance game.
            activeDanceGames[player].StartDanceGame(player, danceMoveSet);
        }
    }
}
