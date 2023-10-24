using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
/// </summary>
[CreateAssetMenu(fileName = "Rooms/EnemySet", menuName = "Rooms/EnemySet")]
public class EnemySet : ScriptableObject
{
    public TipologiaEnemySet tipologiaNemico;
    public TipoSwitchOndata tipoSwitchOndata;
    public int pesoSet;
    public TipoRequisito tipoRequisito;
    public StructPercentualeVitaRimasta valoreVitaRimasta;
    public StructQuantitaVitaMassima valoreQuantitaMassima;
    public StructQuantitaUstioni valoreQuantitaUstioni;
    public StructPerPercentualeCorruzione valoreCorruzione;
    public StructPerTipiPotereStanza valoriPerPoteri;
    public StructPerPoteriSpecificiStanza valoriPerPoteriSpecifici;
    public StructPerEntrataStanze valoriPerEntrataStanze;
    public StructPerConteggioDistanzaPercorsa valoriDistanzaPercorsa;
    public StructPerValoreStatistica valoriStatistichePersonaggio;
    public List<Ondata> listaDiOndate;
    public RequisitoFlag flagsOnSelect;
    public RequisitoFlag flagsOnComplete;
    public bool CanRepeat = true;

    private float m_percentualeTotale = 0;
    private void OnValidate()
    {
        for (int i = 0; i < listaDiOndate.Count; i++)
        {
            var ondata = listaDiOndate[i];
            foreach (var mostro in ondata.mostri)
            {
                m_percentualeTotale += mostro.percentualeSpawnMostri;
            }
            if(m_percentualeTotale > 100)
            {
                Debug.LogError($"<b>Percentuale spawn mostri nello scritpable {this.name} maggiore del 100%, risolvere immediatamente</b>");
            }
            m_percentualeTotale = 0;
        }
    }

    [System.Serializable]
    public struct Ondata
    {
        public int minEnemyOndata;
        public int maxEnemyOndata;
        public List<EnemyQuantity> mostri;
        public List<StaticEnemy> staticMostri;

    }
    [System.Serializable]
    public class EnemyQuantity
    {
        public GameObject nemicoDaIstanziare;
        [Range(1, 100)]
        public float percentualeSpawnMostri = 1;
        public bool canMutate;
        public EDistanzaPlayerDaNemico playerDistance;
    }    
    [System.Serializable]
    public class StaticEnemy
    {
        public GameObject nemicoDaIstanziare;
        public int quantitaDiMostriDaIstanziare = 0;
        public bool canMutate;
        public EDistanzaPlayerDaNemico playerDistance;
    }

}
public enum TipoSwitchOndata
{
    NemiciRimanenti,
    Tempo
}
public enum EDistanzaPlayerDaNemico
{
    NonSpecificato,
    Vicino,
    Intermedia,
    Lontano
}

public enum TipologiaEnemySet
{
    Infetti,
    Umani,
    Bestie,
    BestieInfette,

}