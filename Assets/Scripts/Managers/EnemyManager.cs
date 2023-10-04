using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager m_instance;

    public List<GameObject> m_activeEnemyList;
    public static EnemyManager Instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = FindAnyObjectByType<EnemyManager>();
                if(m_instance == null) 
                {
                    m_instance = new UnityEngine.GameObject("EnemyManager").AddComponent<EnemyManager>();
                }
                
            }
            return m_instance;
        }

    }
    private void Awake()
    {
        GetAllEnemyInScene();
    }
    private void OnDisable() => m_activeEnemyList.Clear();
    private void GetAllEnemyInScene()
    {
        EnemyController[] allEnemiesInScene = FindObjectsByType<EnemyController>(FindObjectsInactive.Include,FindObjectsSortMode.None);
        foreach (var enemy in allEnemiesInScene)
        {
            m_activeEnemyList.Add(enemy.gameObject);
        }

    }

}
