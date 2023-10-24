using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlloMalattiaManager : MonoBehaviour
{
    //dovrà essere inserita la lista di mutazioni e la mutazione attuale
    public Mortality mortality;
    public Infectivity infectivity;
    public Mutability mutability;
    public Resistance resistance;
}

[System.Serializable]
public class MalattiaStat
{
    public string name;
    public string description;
    public int baseValue = 50;
    public int actualValue = 0;
}
[System.Serializable]
public class Mortality : MalattiaStat
{

}
[System.Serializable]
public class Infectivity : MalattiaStat
{

}
[System.Serializable]
public class Mutability : MalattiaStat
{

}
[System.Serializable]
public class Resistance : MalattiaStat
{

}
