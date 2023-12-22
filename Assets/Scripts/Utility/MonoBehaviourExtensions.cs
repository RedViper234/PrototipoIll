using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class MonoBehaviourExtensions{
    public static CoroutineHandle RunCoroutine(this MonoBehaviour owner, IEnumerator coroutine){
        return new CoroutineHandle(owner, coroutine);
    }
}
public static class DictionaryExtensions
{
    // Extension method to randomize a dictionary
    public static Dictionary<TKey, TValue> Randomize<TKey, TValue>(this Dictionary<TKey, TValue> inputDict)
    {
        // Convert dictionary to list for randomization
        List<KeyValuePair<TKey, TValue>> dictList = new List<KeyValuePair<TKey, TValue>>(inputDict);

        // Use Fisher-Yates shuffle algorithm to randomize the list
        System.Random random = new System.Random();
        int n = dictList.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            KeyValuePair<TKey, TValue> value = dictList[k];
            dictList[k] = dictList[n];
            dictList[n] = value;
        }

        // Create a new dictionary from the randomized list
        Dictionary<TKey, TValue> randomizedDict = new Dictionary<TKey, TValue>();
        foreach (var pair in dictList)
        {
            randomizedDict.Add(pair.Key, pair.Value);
        }

        return randomizedDict;
    }
}