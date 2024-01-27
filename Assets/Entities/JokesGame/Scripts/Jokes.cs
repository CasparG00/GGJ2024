using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Joke")]
public class Jokes : ScriptableObject
{
    public List<Joke> jokeCollection;
}

[Serializable]
public class Joke
{
    public int id;
    public string[] jokeSentence;
}
