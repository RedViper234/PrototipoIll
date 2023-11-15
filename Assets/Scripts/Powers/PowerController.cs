using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerController : MonoBehaviour
{
    private static PowerController instance;

    public static PowerController Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject powerControllerObject = new GameObject("PowerController");
                instance = powerControllerObject.AddComponent<PowerController>();
                DontDestroyOnLoad(powerControllerObject);
            }
            return instance;
        }
    }

    void Awake()
    {
        instance = this;
    }

    List<APowers> playerPowers = new List<APowers>();
    
}
