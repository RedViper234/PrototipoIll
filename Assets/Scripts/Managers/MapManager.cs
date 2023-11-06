using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class MapManager : Manager,ISubscriber
{
    private PlayerInput inputActions;
    private UIDocument m_mapDocument;

 
    public void OnPublish(IMessage message)
    {
        if(message is OpenMapMessage){
            OpenMapMessage temp = (OpenMapMessage)message;
            OpenMap(temp.isOpen);
        }
    }

    public void Awake(){
        inputActions = new PlayerInput();
    }
    private void Start() {
        m_mapDocument = GetComponent<UIDocument>();
        Publisher.Publish(new OpenMapMessage(false));
    }





    public void OpenMap(bool isOpen = false){
        VisualElement rootElement = m_mapDocument.rootVisualElement;
        rootElement.style.display = isOpen ? DisplayStyle.Flex : DisplayStyle.None;
        Debug.Log("OPEN MAP: " + rootElement.visible);
    }
    private void OpenMapViaCodice(InputAction.CallbackContext context){
        Debug.LogError("CIAOOOOO");
        if(context.performed){
            OpenMap(m_mapDocument.rootVisualElement.style.display == DisplayStyle.None);
        }
    }



    private void OnEnable() {
        Publisher.Subscribe(this,new OpenMapMessage());
        inputActions.UI.Enable();
        inputActions.UI.OpenMap.performed += OpenMapViaCodice;

    }
    private void OnDisable() {
        Publisher.Unsubscribe(this,new OpenMapMessage());
        inputActions.UI.Disable();
        inputActions.UI.OpenMap.performed -= OpenMapViaCodice;

    }

}