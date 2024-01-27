using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JokesGameSelect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI jokeText;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private Sprite unselectedSprite;

    private JokesGame game;
    private int id;

    public void Refresh(JokesGame _game, int _id, string _text)
    {
        game = _game;
        id = _id;
        jokeText.text = _text;
    }

    public void Select(bool _value)
    {
        buttonImage.sprite = _value ? selectedSprite : unselectedSprite;
    }

    public void NextSentence()
    {
        game.NextSentence(id);
    }
}
