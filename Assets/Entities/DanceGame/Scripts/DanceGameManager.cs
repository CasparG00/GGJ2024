using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

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
    }
    
    private void OnDisable()
    {
        King.OnPreferredActivityChanged -= OnPreferredActivityChanged;
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
            foreach (var element in activeDanceGames)
            {
                bool isDanceGameActive = element.Value != null;
                if (isDanceGameActive)
                {
                    element.Value.EndDanceGame();
                }
            }
        }
    }

    public void OnPlayerJoined(PlayerInput _playerInput)
    {
        Player player = _playerInput.GetComponent<Player>();
        
        if (kingPreferredActivity is Activity.Dance)
        {
            DanceGame danceGame = Instantiate(danceMovePrefab, transform);
            danceGame.StartDanceGame(player, danceMoveSet);
            
            activeDanceGames[player] = danceGame;

            return;
        }
        
        activeDanceGames[player] = null;
    }
    
    public void OnPlayerLeft(PlayerInput _playerInput)
    {
        Player player = _playerInput.GetComponent<Player>();
        
        if (kingPreferredActivity is Activity.Dance)
            activeDanceGames[player].EndDanceGame();

        activeDanceGames.Remove(player);
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
