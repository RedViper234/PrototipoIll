using System;
using UnityEngine;

[System.Serializable]
public struct RaritaEValoreStanza
{
    public RaritaStanza tipoRaritaStanza;
    [Range(0,100)]
    public float percentualeRarita;
}
