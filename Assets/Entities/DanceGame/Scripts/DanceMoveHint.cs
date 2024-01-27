using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DanceMoveHint : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private LayoutElement layoutElement;
    
    [SerializeField] private float animationDuration = 0.2f;
    
    public void TriggerDespawn()
    {
        StartCoroutine(Despawn());
    }
    
    public void SetSprite(Sprite _sprite)
    {
        image.sprite = _sprite;
    }
    
    private IEnumerator Despawn()
    {
        image.gameObject.SetActive(false);
        
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
