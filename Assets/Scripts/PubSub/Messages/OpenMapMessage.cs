using UnityEngine;

public struct OpenMapMessage : IMessage
{
    public bool isOpen;
    public OpenMapMessage(bool isOpen)
    {
        this.isOpen = isOpen;
    }
}