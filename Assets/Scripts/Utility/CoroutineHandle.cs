using System;
using System.Collections;
using UnityEngine;


public class CoroutineHandle : IEnumerator
{
    public bool IsDone{get;private set;}
    public object Current {get;}
    public bool MoveNext() => !IsDone;
    public delegate Action OnCourotineDone();
    public event OnCourotineDone onCourotineDone;
    public void Reset(){}

    public CoroutineHandle(MonoBehaviour owner, IEnumerator couroutine)
    {
        owner.StartCoroutine(Wrap(couroutine));
    }

    private IEnumerator Wrap(IEnumerator coroutine)
    {
        yield return coroutine;
        if(onCourotineDone != null)
            onCourotineDone.Invoke();
        IsDone = true;
    }
}