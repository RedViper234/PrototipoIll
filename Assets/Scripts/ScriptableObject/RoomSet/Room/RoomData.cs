using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
/// </summary>
[CreateAssetMenu(fileName = "Rooms/RoomData", menuName = "Rooms/RoomData")]
public class RoomData : ScriptableObject
{
    public GameObject prefabStanza;

    [SerializeField] public TipiDiStanzaFLag tipiDiStanza;
    [Space(20)]
    // PROPRIETA PER STANZA COMBATTIMENTO
    [SerializeField] public SottoCategoriaStanzaCombattimento tipoStanzaCombattimento;

    // PROPRIETA PER STANZA BOSS
    [SerializeField] public SottoCategoriaStanzaBoss tipoStanzaBoss;

    // PROPRIETA PER STANZA EVENTO
    [SerializeField] public SottoCategoriaStanzaEvento tipoStanzaEvento;
    [SerializeField] public ETipoEventiMercante sottoTipoStanzaMercante;
    [SerializeField] public ETipoEventiStanzaRiposo sottoTipoStanzaRiposo;



    // PROPRIETA PER STANZA STORIA
    [SerializeField] public SottoCategoriaStanzaStoria tipoStanzaStoria;
   

    [Space(30)]
    public List<EnemySet> setDiMostriDellaStanza;
    public List<RequisitiStanza> requisitiStanza;
    [Range(-100,100)]public int prioritaStanza = 0;
    public bool isFirstRoom = false;
    public RaritaEValoreStanza raritaEValoreStanza;
    public RequisitoFlag flagsOnEnter;
    public RequisitoFlag flagsOnComplete;
}
