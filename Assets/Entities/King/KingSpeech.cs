using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KingSpeech : MonoBehaviour
{
    [SerializeField] private Slider sliderHumor;
    [SerializeField] private RectTransform speechBubble;
    [SerializeField] private TMP_Text speechBubbleText;
    
    private readonly Dictionary<Activity, string> eventTextDictionary = new()
    {
        { Activity.Dance, "Dance for me!" },
        { Activity.Fight, "Fight for me!" },
        { Activity.Jest, "Tell me a joke!"},
    };
    
    private readonly WaitForSeconds bubbleDisplayTime = new(2f);

    private void Start()
    {
        sliderHumor.maxValue = King.Instance.MaximumHumor;
    }
    
    private void OnEnable()
    {
        King.OnPreferredActivityChanged += OnPreferredActivityChanged;
    }
    
    private void OnDisable()
    {
        King.OnPreferredActivityChanged -= OnPreferredActivityChanged;
    }

    private void Update()
    {
        sliderHumor.value = King.Instance.CurrentHumor;
    }

    private void OnPreferredActivityChanged(Activity _activity)
    {
        StartCoroutine(ShowSpeechBubble(_activity));
    }
    
    private IEnumerator ShowSpeechBubble(Activity _activity)
    {
        speechBubble.gameObject.SetActive(true);
        speechBubbleText.text = eventTextDictionary[_activity];
        
        yield return bubbleDisplayTime;
        
        speechBubble.gameObject.SetActive(false);
    }
}
