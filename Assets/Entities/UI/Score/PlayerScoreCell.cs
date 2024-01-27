using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerScoreCell : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;

    public void Refresh(int _playerIndex, int _score)
    {
        playerNameText.text = $"Player {_playerIndex + 1}: {_score}";
        StartCoroutine(ScaleTextAnimation());
    }

    private IEnumerator ScaleTextAnimation()
    {
        float elapsedTime = 0f;
        float duration = 0.22f; // Adjust the duration of the animation as needed
        float amplitude = 0.35f; // Adjust the amplitude of the bounce

        Vector3 startScale = Vector3.one;
        Vector3 targetScale = startScale + Vector3.one * amplitude;

        while (elapsedTime < duration)
        {
            playerNameText.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f; // Reset elapsed time for the bounce back

        while (elapsedTime < duration)
        {
            playerNameText.transform.localScale = Vector3.Lerp(targetScale, startScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final scale is exactly the start scale
        playerNameText.transform.localScale = startScale;
    }
}
