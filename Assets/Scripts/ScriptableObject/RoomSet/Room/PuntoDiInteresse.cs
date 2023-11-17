using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
/// </summary>
[CreateAssetMenu(fileName = "Rooms/PuntoDiInteresse", menuName = "Rooms/Punto Di Interesse")]
public class PuntoDiInteresse : ScriptableObject
{
    [Tooltip("Una lista di stanze possibili, la lista può anche essere lasciata vuota, nel qual caso vuol dire che la stanza verrà sicuramente pescata dalla lista di stanze dell’area o da quella di uno delle sue sottoaree a seconda delle specifiche del parametro sottogruppo (vedi sotto). Da notare che le stanze inserite qui dovrebbero essere uniche e non disponibili in altri punti di interesse. Se una stanza in lista presenta la tipologia Combattimento Standard o Boss (default) segnalare un errore in editor (qui si devono trovare solo stanze ben precise e non che richiedono ulteriori randomizzazioni) e a runtime rimuovere tale tipologia dalla lista, se è l’unica tipologia della stanza allora rimuovere la stanza.")]
    public List<RoomData> listaDiStanzeUniche;
    [Tooltip("booleano che indica se il seguente punto di interesse deve selezionare per forza una delle sue stanze che appaiono in lista di stanze uniche ignorando la normale assegnazione delle stanze dell’area. Chiaramente se impostato a true ma la lista è vuota questo parametro verrà ignorato. Default true")]
    public bool spawnAutomatico = true;
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