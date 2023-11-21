using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{

    private void Awake() {
        Debug.LogWarning("OnEnable MapUI");
        Publisher.Publish(new PassGameObjectMessage(gameObject));
        
    }
    private void Start() {
        gameObject.GetComponent<Image>().enabled = false;
        Image[] array = gameObject.transform.GetComponentsInChildren<Image>();
        for (int i = 0; i < array.Length; i++)
        {
            Image item = array[i];
            item.enabled = false;
        }
        TextMeshProUGUI[] array1 = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < array1.Length; i++)
        {
            TextMeshProUGUI item = array1[i];
            item.enabled = false;
        }

        
    }
}
