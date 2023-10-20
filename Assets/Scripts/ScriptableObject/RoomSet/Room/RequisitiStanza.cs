using UnityEngine;
using System;
[CreateAssetMenu(menuName ="Rooms/Requisiti",fileName = "Rooms/Requisiti")]
public class RequisitiStanza : ScriptableObject
{

    //VARIABILI PER CHECK GUARIGIONE/ MALATTIA
    public TipoRequisito tipoRequisito;
    public StructPerPercentualeCorruzione valoreCorruzione;
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
    //VALORE PER QUANTITA MUTAZIONI
    

}
public enum TipoRequisito
{
    PercentualeCorruzione,
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
