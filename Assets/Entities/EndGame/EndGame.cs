using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [SerializeField] private PlayerScoreManager scoreManager;
    [SerializeField] private TextMeshProUGUI player1Text;
    [SerializeField] private TextMeshProUGUI player2Text;

    private List<Player> players = new();

    private int player1Score;
    private int player2Score;

    private Player player;
    private void FixedUpdate()
    {
        
    }

    private void OnEnable()
    {
        //PlayerAssigner.OnPlayerConnected += OnPlayerConnected;

        StartCoroutine(ShowingScore());
        for (int i = 0; i < scoreManager.players.Count; i++)
            Debug.Log(scoreManager.players[i].Score);   
    }

    private IEnumerator ShowingScore()
    {
        yield return new WaitForSeconds(0.5f);

        player1Text.text = "Score : " + player1Score;
        player2Text.text = "Score : " + player2Score;

        player1Text.enabled = true;
        player2Text.enabled = true;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    private void RefreshCells(int _scoreChange)
    {
        for (int i = 0; i < players.Count; i++)
        {
            //playerScoreCells[i].Refresh(players[i].Color, i, players[i].Score);
        }
    }

    private void OnPlayerConnected(Player _player)   
    {
        players.Add(_player);

        _player.OnPlayerScoreChanged += RefreshCells;

        player2Score = players[0].Score;
    }
}
