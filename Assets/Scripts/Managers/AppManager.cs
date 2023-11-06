using Cinemachine;
using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{
    private static AppManager m_instance;
    public GameObject playePrefabReference;
    public GameObject virtualMachineCameraPlayerPrefab;
    public List<ManagerData> m_managersList;

    private GameObject m_currentPlayerInstance;
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
            DontDestroyOnLoad(m_instance.gameObject);
            return m_instance;
        }

    }

    [HideInInspector] public EnemyManager enemyManager;
    [HideInInspector] public RoomManager roomManager;
    [HideInInspector] public FlagManager flagManager;
    [HideInInspector] public PlayerController playerControllerInstance;
    [HideInInspector] public ControlloMalattiaManager controlloMalattiaManager;
    [HideInInspector] public InventoryManager inventoryManager;
    [HideInInspector] public GameManager gameManager;
    [HideInInspector] public MapManager mapManager;
    private void OnEnable()
    {
        m_instance= this;
        SpawnaManagersSeNonCiSono();
        gameManager = GetComponent<GameManager>();
        mapManager = GetComponent<MapManager>();
        enemyManager = GetComponentInChildren<EnemyManager>();
        roomManager = GetComponentInChildren<RoomManager>();
        flagManager = GetComponentInChildren<FlagManager>();
        controlloMalattiaManager = GetComponentInChildren<ControlloMalattiaManager>();
        inventoryManager = GetComponentInChildren<InventoryManager>();
        if (FindAnyObjectByType(typeof(CinemachineVirtualCamera)) != null)
        {
            virtualMachineCameraPlayerPrefab = FindAnyObjectByType<CinemachineVirtualCamera>().gameObject;
        }
        else
        {
            GameObject camera = Instantiate(virtualMachineCameraPlayerPrefab, new Vector3(0,0,0),Quaternion.identity);
            virtualMachineCameraPlayerPrefab = camera;
        }
    }
    private void Awake()
    {
        //if(playePrefabReference != null && playerControllerInstance)
        //{
        //    playerControllerInstance = FindAnyObjectByType(typeof(PlayerController)) as PlayerController;
        //    playePrefabReference = playerControllerInstance.gameObject;
        //}
        //else
        //{
        //    playerControllerInstance = playePrefabReference.GetComponent<PlayerController>();
        //}
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
    public void SetPlayerObject(GameObject player)
    {
        m_currentPlayerInstance = player;
    }
    public void SetCameraPlayer()
    {
        CinemachineVirtualCamera vc = virtualMachineCameraPlayerPrefab.GetComponent<CinemachineVirtualCamera>();
        if (vc != null)
        {
            vc.Follow = m_currentPlayerInstance.transform;
        }
    }

}

public abstract class Manager:MonoBehaviour
{

}

[System.Serializable]
public struct ManagerData 
{
    public GameObject manager;
}
