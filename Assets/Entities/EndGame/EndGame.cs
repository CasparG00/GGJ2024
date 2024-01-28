using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [SerializeField] private GameObject fadeImage;
    [SerializeField] private PlayerScoreManager scoreManager;
    [SerializeField] private TextMeshProUGUI gameOverText;

    [SerializeField] private GameObject playerImage;
    [SerializeField] private TextMeshProUGUI[] playerText;

    private List<Player> players = new();

    private int player1Score;
    private int player2Score;

    private Player player;
    private void FixedUpdate()
    {
        
    }

    private void OnEnable()
    {
        King.KingHappy += KingHappy;
        King.KingAngry += KingAngry;
    }

    private void OnDisable()
    {
        King.KingHappy -= KingHappy;
        King.KingAngry -= KingAngry;    
    }

    private void KingHappy()
    {
        fadeImage.SetActive(true);
        Debug.Log("King is Happy!");
        StartCoroutine(ShowingScore());
    }

    private void KingAngry()
    {
        fadeImage.SetActive(true);
        gameOverText.enabled = true;
        Debug.Log("King is Angry!");
    }

    private IEnumerator ShowingScore()
    {
        playerImage.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < scoreManager.players.Count; i++)
        {
            playerText[i].text = "Score : " + scoreManager.players[i].Score;
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    //private void RefreshCells(int _scoreChange)
    //{
    //    for (int i = 0; i < players.Count; i++)
    //    {
    //        //playerScoreCells[i].Refresh(players[i].Color, i, players[i].Score);
    //    }
    //}

    //private void OnPlayerConnected(Player _player)   
    //{
    //    players.Add(_player);

    //    _player.OnPlayerScoreChanged += RefreshCells;

    //    player2Score = players[0].Score;
    //}
}
