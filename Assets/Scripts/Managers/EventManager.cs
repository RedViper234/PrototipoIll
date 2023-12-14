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
    public static UnityAction<GameObject> HandleOnPlayerHit; //Quando il player colpisce un nemico
    public static UnityAction HandlePlayerDeath;
    public static UnityAction HandleOnEnemyHit; //Quando un nemico colpisce il player
    public static UnityAction HandleOnEnemyDeath;
    public static UnityAction HandlePlayerDash;
    public static UnityAction HandlePlayerJump;
    public static UnityAction HandlePlayerDashEnd;
    public static UnityAction HandlePlayerJumpEnd;
    public static UnityAction<bool> HandlePlayerAttackBegin;
    public static UnityAction HandlePlayerAttackEnd;
    public static UnityAction HandlePlayerSpecialAttack; 
    public static UnityAction HandleEndOfDash; 


    public static void IncrementPassedRooms()
    {
        NPassedRooms++;
    }
}
