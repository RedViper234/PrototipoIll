using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(menuName ="Rooms/Requisiti",fileName = "Rooms/Requisiti")]
public class RequisitiStanza : ScriptableObject
{

    //VARIABILI PER CHECK GUARIGIONE/ MALATTIA
    public TipoRequisito tipoRequisito;
    public StructPerPercentualeCorruzione valoreCorruzione;

    public StructPercentualeVitaRimasta valoreVitaRimasta;

    public StructQuantitaVitaMassima valoreQuantitaVitaMassima;

    public StructQuantitaUstioni valoreQuantitaUstioni;
    // VALORI PER POTERI
    public StructPerTipiPotereStanza valoriPerPoteri;
    // VALORI PER POTERI SPECIFICI
    public StructPerPoteriSpecificiStanza valoriPerPoteriSpecifici;
    // VALORI PER ENTRATA STANZE
    public StructPerEntrataStanze valoriPerEntrataStanze;
    // VALORE PER DISTANZA PERCORSA 
    public StructPerConteggioDistanzaPercorsa valoriDistanzaPercorsa;
    //VALORE PER STATISTICHE PERSONAGGIO
    public StructPerValoreStatistica valoriStatistichePersonaggio;
    //VALORE PER QUANTITA USTIONI
    public List<FlagsSO> flagCheIlPlayerDeveAvere;

}
public enum TipoRequisito
{
    PercentualeCorruzione,
    PercentualeVitaRimasta,
    QuantitaVitaMassima,
    Ustioni,
    NumeroPoteriOttenuti,
    PoterSpecifico,
    NumeroStanzeAttraversate,
    DistanzaPercorsa,
    ValoreDiStatistica,
    MutazioniAccumulate,
    Flag
}
public enum ListaTipiPoteriPerRequisiti
{
    Guarigione,
    Malattia,
    Purificazione,
    Difesa,
    Sopravvivenza,
    Infezione,
    Distruzione, 
    Mutazioni
}
//VERRA RIMOSSA SICURAMENTE
public enum ListaTuttiPoteri
{
    ResistenzaAInfezione,
    ResistenzaAlContagio,
    ResistenzaAlMiasma,
    RicercaDiUnaCura,
    CombattereAlSicuro,
    CombattereGliInfetti,
    Guarigione,
    EliminareLePustole,
    EradicareGliInfetti,
    Tentazione,
    PianoDiEmergenza,
    PunireGliInfetti,
    Ambivalenza,
    Purificare,
    Ripresa,
    Incassare,
    Ripicca,
    Resistenza,
    Tenacia,
    Intercettare,
    Respinta,
    ManovraEvasiva,
    Vendetta,
    Esca,
    MettersiAlSicuro,
    Furia,
    Rallentamento,
    NascondersiNeiMiasmi,

}
