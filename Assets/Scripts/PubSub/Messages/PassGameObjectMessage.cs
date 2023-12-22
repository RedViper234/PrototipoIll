using UnityEngine;

public struct PassGameObjectMessage : IMessage
{
    public GameObject objectToPass;
    public PassGameObjectMessage(GameObject objectToPass)
    {
        this.objectToPass = objectToPass;
    }
}