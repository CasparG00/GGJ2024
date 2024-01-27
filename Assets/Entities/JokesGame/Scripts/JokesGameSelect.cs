using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JokesGameSelect : MonoBehaviour
{
    [SerializeField] private Text jokeText;

    public int id;

    private void Awake()
    {

    }

    public void ChangeText(string sentence)
    {
        jokeText.text = sentence;
    }

    public void NextSentence()
    {
        JokesGame.instance.NextSentence(jokeText.text, id);
    }
}
