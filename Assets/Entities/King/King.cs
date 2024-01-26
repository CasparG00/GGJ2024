using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Activity
{
    Idle,
    Dance,
    Jest,
    Fight,
}

public class King : MonoBehaviour
{
    // This event gets called every time the king changes his preferred activity.
    // The player listens to this event to update the player's knowledge of the king's preferred activity.
    // The UI can also listen to this event to update the UI.
    // There's only one king so a static event is fine.
    public static event Action<Activity> OnPreferredActivityChanged;
    
    public int Humor { get; private set; }

    private List<Activity> preferredActivitySet;

    // The minimum and maximum amount of time the king will like an activity before changing his mind.
    [SerializeField] private Vector2Int attentionSpanRange = Vector2Int.one;
    
    // The index of the current preferred activity in the list.
    private int preferredActivityIndex;

    private void Awake()
    {
        CreatePreferredActivityList();
        StartCoroutine(DequeuePreferredActivity());
    }
    
    private IEnumerator DequeuePreferredActivity()
    {
        while (Humor > 0)
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
        
        for (int i = 0; i < 20; i++)
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
}
