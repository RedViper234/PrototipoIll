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
    public List<SpawnRicompensa> spawnRicompense;
    public int numeroRicompenseSpawn;
    [Tooltip(" bool che indica che lo stesso elemento della lista ricompense può essere selezionato più di una volta.")]
    public bool accettaRipetizioneRicompense;
    public ERespawn respawn;
    public int respawnLimit;
    [TextArea]
    public string descrizione;
    [Range(-1, 100)]
    public int movementCost = -1;
    [Range(-1,100)]
    public int secTimeBattleProbability = -1;
    public List<ConnectedPoints> points;  
    public List<StructPerListaFlags> flagOnEnter;
    public bool flagOnEnterOnlyFirstTime;
    public bool blockSpecialRoom;
}
[System.Serializable]
public class ConnectedPoints
{
    public List<PuntoDiInteresse> points;
    public bool breakable;
    public bool monouso;
}
[System.Serializable]
public class SpawnRicompensa
{
    public TipoRicompensa tipoRicompensa;
    public int dropMinimo;
    public int dropMassimo;
}
public enum TipoRicompensa
{
    NessunaRicompensa,
    Potere,
    StatFragment,
    EvoluzionePotere,
    PotereMaggioreCasuale,
    PotereEpico,
    Drop,
    DropMaggioreCasuale,
    DropMinoreCasuale

}
public enum ERespawn
{
    No,
    SoloStanzaEsaurimento,
    SoloStanzaRipetibile,
    SiaStanzaCheRicompensaEsaurimento,
    SiaStanzaCheRicompensaRipetibile

   
}