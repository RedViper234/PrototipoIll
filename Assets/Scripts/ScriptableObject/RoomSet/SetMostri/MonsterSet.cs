using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
/// </summary>
[CreateAssetMenu(fileName = "Rooms/MonsterSet", menuName = "Rooms/MonsterSet")]
public class MonsterSet : ScriptableObject
{
    public List<SetMostri> listaDiNemiciDaSpawnareInStanza;

    [System.Serializable]
    public struct SetMostri
    {
        public List<GameObject> mostri;
    }
}
