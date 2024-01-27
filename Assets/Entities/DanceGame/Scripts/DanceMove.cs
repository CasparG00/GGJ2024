using UnityEngine;

// The struct is used as a workaround for Unity not being able to serialize dictionaries.
[System.Serializable]
public struct DanceMove
{
    public int id;
    public Sprite sprite;
}
