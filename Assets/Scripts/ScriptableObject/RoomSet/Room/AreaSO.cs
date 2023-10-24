using System;
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
    public int breakPointsMin;
    public int breakPointsMax;
    public float secondTimeBattleProbablity = -1;
    public List<StructPerListaFlags> onCompleteFlags;
    public List<SpecialRoom> stanzeSpeciali;
    public bool onlyOneSpecialRoom = false;
    public List<EnemySet> enemySet;
    public List<RoomDistribution> roomDistributions;


    private void OnValidate()
    {
        foreach (var item in roomDistributions)
        {
            if(item.minRespawn > item.maxRespawn)
            {
                item.maxRespawn = item.minRespawn;
            }
        }
    }
}
[Serializable]
public struct SpecialRoom
{
    public RoomData stanza;
    [Range(-100,100)]
    public int priorit‡StanzaSpeciale;
    public bool riproduzioneMultipla;
}
[Serializable]
public class RoomDistribution
{
    public TipiDiStanza tipoStanzaArea;
    [Space]
    public SottoCategoriaStanzaCombattimento tipoStanzaCombattimentoArea;
    public SottoCategoriaStanzaBoss tipoStanzaBossArea;
    // PROPRIETA PER STANZA EVENTO
    [Space]
    public SottoCategoriaStanzaEvento tipoStanzaEvento;
    public ETipoEventiMercante sottoTipoStanzaMercante;
    public ETipoEventiStanzaRiposo sottoTipoStanzaRiposo;
    [Space]
    public SottoCategoriaStanzaStoria tipoStanzaStoriaArea;
    [Space]
    public int min;
    public int max;
    [Space]
    [Range(-1,100)]public int minRespawn;
    [Range(-1,100)]public int maxRespawn;
    public List<SpawnRicompensa> tipoRicompensa;
    public int numeroRicompense;
    public bool accettaRipetizioniRicompense;
}
public class PercentualRoomDistribution
{
    public TipiDiStanza tipoStanzaArea;
    [Space]
    public SottoCategoriaStanzaCombattimento tipoStanzaCombattimentoArea;
    public SottoCategoriaStanzaBoss tipoStanzaBossArea;
    // PROPRIETA PER STANZA EVENTO
    [Space]
    public SottoCategoriaStanzaEvento tipoStanzaEvento;
    public ETipoEventiMercante sottoTipoStanzaMercante;
    public ETipoEventiStanzaRiposo sottoTipoStanzaRiposo;
    [Space]
    public SottoCategoriaStanzaStoria tipoStanzaStoriaArea;
    [Space]
    public int peso = 0;
    public int maxAppereance;
    public RoomData defaultRoom;
}