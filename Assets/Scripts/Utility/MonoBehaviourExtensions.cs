using UnityEngine;
using System;
using System.Collections;

public static class MonoBehaviourExtensions{
    public static CoroutineHandle RunCoroutine(this MonoBehaviour owner, IEnumerator coroutine){
        return new CoroutineHandle(owner, coroutine);
    }
}