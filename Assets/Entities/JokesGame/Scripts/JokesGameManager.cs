using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JokesGameManager : MonoBehaviour
{
    [SerializeField] private JokesGame jokesGamePrefab;
    
    private readonly Dictionary<Player, JokesGame> activeJokeGames = new();
    
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
        King.KingHappy += OnGameOver;
        King.KingAngry += OnGameOver;
        
        PlayerAssigner.OnPlayerConnected -= OnPlayerConnected;
        PlayerAssigner.OnPlayerDisconnected -= OnPlayerDisconnected;
    }
    
    private void OnGameOver()
    {
        EndAllJokesGames();
    }
    
    private void OnPreferredActivityChanged(Activity _activity)
    {
        kingPreferredActivity = _activity;
        
        if (kingPreferredActivity is Activity.Jest)
        {
            List<Player> players = activeJokeGames.Keys.ToList();
            foreach (var player in players)
            {
                // If a player has left the dance game will be null.
                // In this case a new one needs to be created.
                if (activeJokeGames[player] == null)
                {
                    JokesGame jokesGame = Instantiate(jokesGamePrefab, transform);
                    activeJokeGames[player] = jokesGame;
                }
            
                // Reinitialize the dance game.
                activeJokeGames[player].StartJokesGame(player);
            }
        }
        else
        {
            EndAllJokesGames();
        }
    }
    
    private void EndAllJokesGames()
    {
        foreach (var element in activeJokeGames)
        {
            bool isGameActive = element.Value != null;
            if (isGameActive)
            {
                element.Value.EndJokesGame();
            }
        }
    }
    
    private void OnPlayerConnected(Player _player)
    {
        if (kingPreferredActivity is Activity.Jest)
        {
            JokesGame jokesGame = Instantiate(jokesGamePrefab, transform);
            jokesGame.StartJokesGame(_player);
            
            activeJokeGames[_player] = jokesGame;

            return;
        }
        
        activeJokeGames[_player] = null;
    }
    
    public void OnPlayerDisconnected(Player _player)
    {
        if (kingPreferredActivity is Activity.Jest)
            activeJokeGames[_player].EndJokesGame();

        activeJokeGames.Remove(_player);
    }
}
