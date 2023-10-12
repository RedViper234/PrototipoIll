using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
/// </summary>
[CreateAssetMenu(fileName = "Rooms/PuntoDiInteresse", menuName = "Rooms/Punto Di Interesse")]
public class PuntoDiInteresse : ScriptableObject
{
    public List<RoomData> roomPossibiliDaSpawnare;
    public bool spawnAutomatico;
    [TextArea]
    public string descrizione;
    [Range(-1, 100)]
    public int movementCost = -1;
    [Range(-1,100)]
    public int secTimeBattleProbability = -1;
    public List<ConnectedPoints> points;  
    public List<StructPerListaFlags> flagOnEnter;
}
[System.Serializable]
public struct ConnectedPoints
{
    public List<PuntoDiInteresse> points;
    public bool breakable;
    public bool monouso;
}