using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{
    private static AppManager m_instance;
    public List<ManagerData> m_managersList;
    public static AppManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindAnyObjectByType<AppManager>();
                if (m_instance == null)
                {
                    m_instance = new UnityEngine.GameObject("AppManager").AddComponent<AppManager>();
                }

            }
            return m_instance;
        }

    }

    [HideInInspector] public EnemyManager enemyManager;
    private void OnEnable()
    {
        m_instance= this;
        SpawnaManagersSeNonCiSono();
        enemyManager = GetComponentInChildren<EnemyManager>();
      
    }

    public void SpawnaManagersSeNonCiSono()
    {
        if(m_managersList != null)
        {
            foreach (ManagerData manager in m_managersList)
            {
                if (!GameObject.Find(manager.manager.name))
                {
                    GameObject managerInstanziato = Instantiate(manager.manager,gameObject.transform);
                    managerInstanziato.transform.SetParent(transform,false);
                }
                else
                    continue;
            }

        }
    }
    private void OnDisable()
    {
        if (!this.gameObject.scene.isLoaded) return;
        m_managersList = null;
        transform.DetachChildren();
    }
}

public abstract class Manager:MonoBehaviour
{

}
[System.Serializable]
public struct ManagerData 
{
    public GameObject manager;
    public float Ciao;
}
