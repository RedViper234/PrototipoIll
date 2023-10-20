using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
/// </summary>
[CreateAssetMenu(fileName = "Rooms/EnemySet", menuName = "Rooms/EnemySet")]
public class EnemySet : ScriptableObject
{
    public TipoSwitchOndata tipoSwitchOndata;
    public int pesoSet;
    public TipoRequisito tipoRequisito;
    public StructPerPercentualeCorruzione valoreCorruzione;
    public StructPerTipiPotereStanza valoriPerPoteri;
    public StructPerPoteriSpecificiStanza valoriPerPoteriSpecifici;
    public StructPerEntrataStanze valoriPerEntrataStanze;
    public StructPerConteggioDistanzaPercorsa valoriDistanzaPercorsa;
    public StructPerValoreStatistica valoriStatistichePersonaggio;
    public List<Ondata> listaDiNemiciDaSpawnareInStanza;
    public List<FlagsSO> flagOnSelect; 
    public List<FlagsSO> flagOnComplete;
    public bool CanRepeat = true;

    [System.Serializable]
    public struct Ondata
    {
        public List<GameObject> mostri;

    }
}
public enum TipoSwitchOndata
{
    NemiciRimanenti,
    Tempo
}
