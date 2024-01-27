using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class King : MonoBehaviour
{

    /// <summary>
    /// This event gets called every time the king changes his preferred activity.
    /// The player listens to this event to update the player's knowledge of the king's preferred activity.
    /// The UI can also listen to this event to update the UI. 
    /// There's only one king so a static event is fine. 
    /// </summary>
    public static event Action<Activity> OnPreferredActivityChanged;
    private List<Activity> preferredActivitySet;

    // The index of the current preferred activity in the list.
    private int preferredActivityIndex; 

    // The minimum and maximum amount of time the king will like an activity before changing his mind.
    [SerializeField] private Vector2Int attentionSpanRange = Vector2Int.one;  

    //Player Settings
    private static readonly List<Player> playerList = new();
    public float CurrentHumor { get; private set; }

    [SerializeField] private int decreaseRate = 1;
    [SerializeField] private int activitySize = 20;

    //Make the King a Singleton, so stuff does not need to be static.
    private static King Instance { get; set; }

    private void Awake()
    {
        #region singleton
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        #endregion singleton

        CreatePreferredActivityList();
        StartCoroutine(DequeuePreferredActivity());
    }

  

    //When enabling the King AI make sure to subscribe the Player again.
    private void OnEnable()
    {
        foreach (var _player in playerList)
        {
            _player.OnPlayerScoreChanged += OnPlayerScoreChanged;
        }
    }

    private void OnDisable()
    {
        foreach (var _player in playerList)
        {
            _player.OnPlayerScoreChanged -= OnPlayerScoreChanged;
        }
    }

    private void Update()
    {
        HandleHumor();
       
    }

    private IEnumerator DequeuePreferredActivity()
    {
        while (CurrentHumor > 0)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(attentionSpanRange.x, attentionSpanRange.y));
            
            // Add one to the index.
            // Then use the modulo to set the index back to 0 if it's greater than the length of the list.
            preferredActivityIndex = preferredActivityIndex++ % preferredActivitySet.Count;
            
            OnPreferredActivityChanged?.Invoke(preferredActivitySet[preferredActivityIndex]);
        }
    }

    private void CreatePreferredActivityList()
    {
        preferredActivitySet = new List<Activity>();
        
        for (int i = 0; i < activitySize; i++)
        {
            // Every loop we create a list of all possible activities.
            List<Activity> possibleActivities = new((Activity[])Enum.GetValues(typeof(Activity)));

            // Then we remove the idle activity because the king does not like idling.
            possibleActivities.Remove(Activity.Idle);
            
            // Then we remove the last activity in the preferred activity set.
            // This is to prevent the king from liking the same activity twice in a row.
            if (preferredActivitySet.Count > 0)
                possibleActivities.Remove(preferredActivitySet.Last());
            
            // Then we choose a random activity from the list that's left to add to the set.
            preferredActivitySet.Add(possibleActivities[UnityEngine.Random.Range(0, possibleActivities.Count)]);
        }
    }

    //Subscribe the player to the list so the King can access it.
    public static void AddPlayer(Player _player)
    {
        playerList.Add(_player);
        _player.OnPlayerScoreChanged += OnPlayerScoreChanged;
    }

    //Remove the player from the list so the King won't look for it.
    public static void RemovePlayer(Player _player)
    {
        playerList.Remove(_player);
        _player.OnPlayerScoreChanged -= OnPlayerScoreChanged;
    }

    //Add score to the HumorMeter
    private static void OnPlayerScoreChanged(int _scoreChange)
    {
        Instance.CurrentHumor += _scoreChange;
    }

    private void HandleHumor()
    {
        CurrentHumor -= decreaseRate * Time.deltaTime;
    }

}
