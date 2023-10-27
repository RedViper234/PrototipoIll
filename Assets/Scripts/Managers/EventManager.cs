using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    public static UnityAction HandleBeginRoom;
    public static UnityAction HandleEndRoom;
    public static UnityAction HandleBeginOfFight;
    public static UnityAction HandleEndOfFight;
    public static UnityAction HandleDeath;
    public static UnityAction<int> HandleEveryNRoom;
    public static UnityAction<float> HandleEveryNTime;

}
