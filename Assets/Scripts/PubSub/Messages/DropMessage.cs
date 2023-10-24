using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DropMessage : IMessage
{
    public storedDrop drop;
    public operation operation;

    public DropMessage(storedDrop drop, operation operation)
    {
        this.drop = drop;
        this.operation = operation;
    }
}

public enum operation
{
    Add,
    Subtract,
}
