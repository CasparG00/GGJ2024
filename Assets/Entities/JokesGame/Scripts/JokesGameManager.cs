using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JokesGameManager : MonoBehaviour
{
    public static JokesGameManager instance;

    [SerializeField] private Text scoreText;
    private int score = 0;

    private void Awake()
    {
        instance = this;
        TextChange();
    }

    public void AddScore(int s)
    {
        score += s;
        TextChange();
    }

    private void TextChange()
    {
        scoreText.text = "SCORE : " + score;
    }
}
