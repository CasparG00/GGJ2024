using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JokesGame : MonoBehaviour
{
    public static JokesGame instance;

    [SerializeField] private Jokes joke;

    [SerializeField] private int maximumJokeDialog = 3;
    [SerializeField] private GameObject selectButton;

    public List<JokesGameSelect> selectButtonList;

    private List<Joke> jokeList;

    private List<Joke> selectedJockList = new List<Joke>();

    private int curSentense = 0;

    private int curId = 0;

    private void Awake()
    {
        instance = this;

        selectButtonList = new List<JokesGameSelect>();

        for (int index = 0; index < maximumJokeDialog; index++)
        {
            GameObject button = Instantiate(selectButton);
            button.transform.parent = transform;
            selectButtonList.Add(button.GetComponent<JokesGameSelect>());
        }
    }

    private void Start()
    {
        SelectJokes();
    }

    public void NextSentence(string sentence, int id)
    {
        curSentense += 1;

        if (curId != 0)
        {
            if (curId == id)
            {
                JokesGameManager.instance.AddScore(20);
            }
            else
            {
                JokesGameManager.instance.AddScore(-10);
            }
        }
        curId = id;

        Debug.Log(sentence);

        ChangeJoke();
    }

    public void ChangeJoke()
    {
        if (curSentense > 2)
            return;

        selectedJockList = selectedJockList.OrderBy(_ => Random.value).ToList<Joke>();

        for (int i = 0; i < selectButtonList.Count; i++)
        {
            selectButtonList[i].ChangeText(selectedJockList[i].jokeSentence[curSentense]);
            selectButtonList[i].id = selectedJockList[i].id;
        }
    }

    public void SelectJokes()
    {
        SelectOtherRandom(joke.jokeCollection.Count);

        for (int i = 0; i < maximumJokeDialog; i++)
        {
            selectButtonList[i].ChangeText(jokeList[i].jokeSentence[curSentense]);
            selectButtonList[i].id = jokeList[i].id;
            selectedJockList.Add(jokeList[i]);
        }
    }

    private void SelectOtherRandom(int maxNumber)
    {
        jokeList = new List<Joke>();
        
        for (int i = 0; i < maxNumber; i++)
        {
            jokeList.Add(joke.jokeCollection[i]);
        }

        jokeList = jokeList.OrderBy(_ => Random.value).ToList<Joke>();
    }
}
