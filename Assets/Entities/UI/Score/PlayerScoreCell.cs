using TMPro;
using UnityEngine;

public class PlayerScoreCell : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;

    public void Refresh(int _playerIndex, int _score)
    {
        playerNameText.text = $"Player {_playerIndex + 1}: {_score}";
    }
}
