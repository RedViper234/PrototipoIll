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
    public List<RoomData> roomList;
    public List<StaticRoomDistribution> roomDistributions;
    public List<PercentualRoomDistribution> percentualRoomDistributions;
    [Min(0)]
    public int minStaticRoom = 0;
    [Min(0)]
    public int maxStaticRoom = 1;
    public List<AreaSpawnStandardProbability> standardCombatProbability;
    public RoomData defaultRoom;
    public float mutationProbabilityVariance = 0;


    private void OnValidate()
    {
        foreach (var item in roomDistributions)
        {
            if(item.minRespawn > item.maxRespawn)
            {
                item.maxRespawn = item.minRespawn+1;
            }
        }
    }
}
[Serializable]
public struct SpecialRoom
{
    [Expandable]
    public RoomData stanza;
    [Expandable]
    public RequisitiStanza requisiti;
    [Range(-100, 100)]
    public int prioritaStanzaSpeciale;
    public bool riproduzioneMultipla;
}
[Serializable]
public class StaticRoomDistribution
{
    [Header("TIPOLOGIA STANZA")]
    public TipiDiStanza tipoStanzaArea;
    [Space]
    [Header("SOTTO CATEGORIE")]
    public SottoCategoriaStanzaCombattimento combattimento;
    public SottoCategoriaStanzaBoss boss;
    // PROPRIETA PER STANZA EVENTO
    public SottoCategoriaStanzaEvento evento;
    public SottoCategoriaStanzaStoria storia;
    [Space]
    public int min;
    public int max;
    [Space]
    [Range(-1,100)]public int minRespawn;
    [Range(-1,100)]public int maxRespawn;
    public List<SpawnRicompensa> tipoRicompensa;
    public int numeroRicompense;
    public bool permettiRipetizioniRicompense = true;
}
[Serializable]
public class PercentualRoomDistribution
{
    [Header("TIPOLOGIA STANZA")]
    public TipiDiStanza tipoStanzaArea;
    [Space]
    [Header("SOTTO CATEGORIE")]
    public SottoCategoriaStanzaCombattimento combattimento;
    public SottoCategoriaStanzaBoss boss;
    // PROPRIETA PER STANZA EVENTO
    public SottoCategoriaStanzaEvento evento;
    public SottoCategoriaStanzaStoria storia;
    [Space]
    public int peso = 0;
    public int maxAppereance;
}

public class AreaSpawnStandardProbability
{
    public TipologiaEnemySet tipo;
    public float percentualValue;
}