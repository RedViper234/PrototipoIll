using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Manager
{

    public List<GameObject> m_activeEnemyList;


    private void OnDisable() => m_activeEnemyList = null;
    public void GetAllEnemyInScene()
    {
        EnemyController[] allEnemiesInScene = FindObjectsByType<EnemyController>(FindObjectsInactive.Include,FindObjectsSortMode.None);
        foreach (var enemy in allEnemiesInScene)
        {
            m_activeEnemyList.Add(enemy.gameObject);
        }

    }

    public void RemoveEnemyFromScene(EnemyController controller)
    {
        if(controller != null)
            m_activeEnemyList.Remove(controller.gameObject);
    }
}
