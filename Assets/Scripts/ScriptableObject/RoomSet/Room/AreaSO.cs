using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
/// </summary>
[CreateAssetMenu(fileName = "Rooms/Area", menuName = "Rooms/AreaSO")]
public class AreaSO : ScriptableObject
{
    public List<PuntoDiInteresse> puntiDiInteresse;
    [TextArea]
    public string descrizioneArea;
    public List<AreaSO> sottoAree;
    [Range(-1f, 100f)]
    public float movementCost = -1;
    [Range(-1f, 100f)]
    public float secondTimeBattleProbablity = -1;
    public List<StructPerListaFlags> onCompleteFlags;
    public bool onlyOneSpecialRoom = false;
    public List<MonsterSet> enemySet;
}
public struct SpecialRoom
{
    public RoomData stanzaPrefab;
    [Range(-100,100)]
    public int priorit‡StanzaSpeciale;
    public bool riproduzioneMultipla;
}
public struct RoomDistribution
{
    public TipiDiStanza tipoStanzaArea;
    [Space(20)]
    List<SottoCategoriaRoomDistribution> sottoCategoriaStanza;
   
}
public struct SottoCategoriaRoomDistribution
{
    public SottoCategoriaStanzaCombattimento tipoStanzaCombattimentoArea;
    public SottoCategoriaStanzaBoss tipoStanzaBossArea;
    public SottoCategoriaStanzaEvento tipoStanzaEventoArea;
    public SottoCategoriaStanzaStoria tipoStanzaStoriaArea;
}
