using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class JokesGame : MonoBehaviour
{
    [SerializeField] private Jokes joke;

    [SerializeField] private int maximumJokeDialog = 3;
    [SerializeField] private JokesGameSelect selectButton;

    private Player owner;
    
    private List<JokesGameSelect> selectButtonList;
    private int currentSelectButtonIndex;
    
    private List<Joke> selectedJokeList = new();

    private int currentSentence;
    private int currentId;

    private void Update()
    {
        if (owner is null)
            return;

        if (selectButtonList is { Count: 0 })
            return;
        
        InputAction moveAction = owner.PlayerInput.actions.FindAction("Move");
        if (moveAction.triggered)
        {
            float input = moveAction.ReadValue<Vector2>().y;
            if (input < -0.1f)
            {
                currentSelectButtonIndex = (currentSelectButtonIndex + 1) % selectButtonList.Count;

                UpdateSelectedButton();
            }
            else if (input > 0.1f)
            {
                currentSelectButtonIndex--;

                if (currentSelectButtonIndex < 0)
                {
                    currentSelectButtonIndex = selectButtonList.Count - 1;
                }

                UpdateSelectedButton();
            }
        }

        if (owner.PlayerInput.actions.FindAction("Punch").triggered)
        {
            selectButtonList[currentSelectButtonIndex].NextSentence();
        }
    }
    
    private void UpdateSelectedButton()
    {
        for (int i = 0; i < selectButtonList.Count; i++)
        {
            selectButtonList[i].Select(i == currentSelectButtonIndex);
        }
    }
    
    public void StartJokesGame(Player _owner)
    {
        owner = _owner;
        
        selectButtonList ??= new List<JokesGameSelect>();

        for (int i = selectButtonList.Count - 1; i >= 0; i--)
        {
            Destroy(selectButtonList[i].gameObject);
            selectButtonList.RemoveAt(i);
        }

        for (int i = 0; i < maximumJokeDialog; i++)
        {
            JokesGameSelect button = Instantiate(selectButton, transform);
            selectButtonList.Add(button);
        }
        
        currentSelectButtonIndex = 0;
        
        selectedJokeList.Clear();
        
        currentSentence = 0;
        currentId = 0;
        
        List<Joke> jokeList = SelectOtherRandom();

        for (int i = 0; i < maximumJokeDialog; i++)
        {
            selectButtonList[i].Refresh(this, jokeList[i].id, jokeList[i].jokeSentence[currentSentence]);
            selectedJokeList.Add(jokeList[i]);
        }
    }
    
    public void EndJokesGame()
    {
        Destroy(gameObject);
    }
    
    public void NextSentence(int _id)
    {
        currentSentence += 1;
        
        if (currentId != 0)
        {
            if (currentId == _id)
            {
                owner.AddScore();
            }
            else
            {
                owner.TryHit();
            }
        }
        
        currentId = _id;

        if (currentSentence < 3)
        {
            NextSection();
        }
        else
        {
            StartJokesGame(owner);
        }
    }

    public void NextSection()
    {
        selectedJokeList = selectedJokeList.OrderBy(_ => Random.value).ToList();

        for (int i = 0; i < selectButtonList.Count; i++)
        {
            selectButtonList[i].Refresh(this, selectedJokeList[i].id, selectedJokeList[i].jokeSentence[currentSentence]);
        }
    }

    private List<Joke> SelectOtherRandom()
    {
        List<Joke> jokeList = new(joke.jokeCollection);
        return jokeList.OrderBy(_ => Random.value).ToList();
    }
}
