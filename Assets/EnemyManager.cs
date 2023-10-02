using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager m_instance;

    public List<BaseEnemyController> m_activeEnemyList;
    public static EnemyManager Instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = FindAnyObjectByType<EnemyManager>();
                if(m_instance == null) 
                {
                    m_instance = new GameObject("EnemyManager").AddComponent<EnemyManager>();
                }
                
            }
            return m_instance;
        }

    }
    private void Awake()
    {
        GetAllEnemyInScene();
    }

    private void GetAllEnemyInScene()
    {
        //BaseEnemyController[] allEnemiesInScene = FindObjectsOfTypeAll(typeof BaseEnemyController));
    }
}
