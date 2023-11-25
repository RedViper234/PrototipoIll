using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PowerController : MonoBehaviour
{
    public List<APowers> playerPowers = new List<APowers>();
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

    void OnTriggerEnter2D(Collider2D other)
    {
        //Tasto di interazione

        APowers power = other.GetComponent<APowers>();

        if(power != null) 
        {
            if (!playerPowers.Contains(power))
            {
                playerPowers.Add(power);
            }
        }
    }
}
