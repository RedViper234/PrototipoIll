using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    static int NPassedRooms = 0;

    public static UnityAction HandleBeginRoom;
    public static UnityAction HandleEndRoom;
    public static UnityAction HandleBeginOfFight;
    public static UnityAction HandleEndOfFight;
    public static UnityAction HandleDeath;
    public static UnityAction<int> HandleEveryNRoom;
    public static UnityAction<float> HandleEveryNTime;
    public static UnityAction HandleOnPlayerHit;
    public static UnityAction HandleOnPlayerDie;
    public static UnityAction HandleOnEnemyHit;
    public static UnityAction HandleOnEnemyDie;

    public static void IncrementPassedRooms()
    {
        NPassedRooms++;
    }
}
