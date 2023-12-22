using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : Manager
{

    public List<GameObject> activeEnemyList;
    [SerializeField,MyReadOnly]private List<GameObject> m_privateEnemyList;

    public UnityEvent OnDestroyedEnemy = new();
    private void OnDisable() => activeEnemyList = null;
    public void GetAllEnemyInScene()
    {
        EnemyController[] allEnemiesInScene = FindObjectsByType<EnemyController>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var enemy in allEnemiesInScene)
        {
            activeEnemyList.Add(enemy.gameObject);
        }

    }
    public void AddEnemyToLists(GameObject enemy){
        m_privateEnemyList.Add(enemy);
        if(!activeEnemyList.Contains(enemy)){
            activeEnemyList.Add(enemy);
        }
    }
    public void RemoveEnemyFromScene(EnemyController controller)
    {
        if (controller != null){
            m_privateEnemyList.Remove(controller.gameObject);
            if(m_privateEnemyList.Count == 0){
                AppManager.Instance.roomManager.VaiAvantiDiOndata();
            }
        }
    }

    public void RemoveEveryEnemyFromScene(){
        foreach (var enemy in activeEnemyList){
            Destroy(enemy);
        }
    }
    public void RemoveEveryEnemyFromTheList(){
        m_privateEnemyList.Clear();
        activeEnemyList.Clear();
    }
    private void Start()
    {
        GetAllEnemyInScene();
    }
    
}
