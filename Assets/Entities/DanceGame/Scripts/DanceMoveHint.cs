using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DanceMoveHint : MonoBehaviour
{
    [SerializeField] private Image hintImage;
    [SerializeField] private Image buttonImage;

    [SerializeField] private LayoutElement layoutElement;
    
    [SerializeField] private float animationDuration = 0.2f;
    
    public void TriggerDespawn()
    {
        StartCoroutine(Despawn());
    }
    
    public void SetDanceMoveSprites(DanceMove _danceMove, Player _player)
    {
        hintImage.sprite = _danceMove.sprite;
        
        if(_player.PlayerInput.currentControlScheme == "Keyboard")
        {
            buttonImage.sprite = _danceMove.keyboardButton;
        }
        else
        {
            buttonImage.sprite = _danceMove.controllerButton;
        }
    }
    public void SetButtonSprite(Sprite _sprite)
    {
        buttonImage.sprite = _sprite;
    }

    private IEnumerator Despawn()
    {
        hintImage.gameObject.SetActive(false);
        
        float startHeight = layoutElement.preferredHeight;
        
        float time = 0f;
        while (time < animationDuration)
        {
            layoutElement.preferredHeight = Mathf.Lerp(startHeight, 0f, time / animationDuration);
            time += Time.deltaTime;
            
            yield return null;
        }
        
        Destroy(gameObject);
    }
}
