using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryManager : MonoBehaviour, ISubscriber
{
    public List<storedDrop> storedDrop = new();
    public void OnPublish(IMessage message)
    {
        DropMessage mess = (DropMessage)message;
        switch (mess.operation)
        {
            case operation.Add:
                AddDrop(mess.drop);
                break;
            case operation.Subtract:
                RemoveDrop(mess.drop);
                break;
            default:
                break;
        }
    }

    void OnEnable()
    {
        Publisher.Subscribe(this, new DropMessage());
    }

    private void OnDisable()
    {
        Publisher.Unsubscribe(this, new DropMessage());
    }

    public void AddDrop(storedDrop dropToAdd)
    {
        if (storedDrop.Find(f => f.dropType == dropToAdd.dropType)!= null)
        {
            storedDrop.Find(f => f.dropType == dropToAdd.dropType).dropQuantity += Mathf.Abs(dropToAdd.dropQuantity);
        }
        else
        {
            storedDrop.Add(dropToAdd);
        }
    }

    public void RemoveDrop(storedDrop dropToRemove)
    {
        if (storedDrop.Find(f => f.dropType == dropToRemove.dropType) != null)
        {
            storedDrop.Find(f => f.dropType == dropToRemove.dropType).dropQuantity -= Mathf.Abs(dropToRemove.dropQuantity);
            if (storedDrop.Find(f => f.dropType == dropToRemove.dropType).dropQuantity <= 0)
            {
                storedDrop.Remove(storedDrop.Find(f => f.dropType == dropToRemove.dropType));
            }
        }
        else
        {
            
        }
    }

    public int checkDropQuantity(storableDrop dropToCheck)
    {
        return (storedDrop.Find(f => f.dropType == dropToCheck) != null ? storedDrop.Find(f => f.dropType == dropToCheck).dropQuantity : 0);
    }

}

public enum storableDrop
{
    Dna,
    Corpi,
    Cuori,
    Armi
}

public class storedDrop
{
    public storableDrop dropType;
    public int dropQuantity;
}
