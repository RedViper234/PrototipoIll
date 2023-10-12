using UnityEngine;
using System;
[CreateAssetMenu(menuName ="Rooms/Requisiti",fileName = "Rooms/Requisiti")]
public class RequisitiStanza : ScriptableObject
{

    //VARIABILI PER CHECK GUARIGIONE/ MALATTIA
    public TipoRequisito tipoRequisito;
    //VALORI MALATTIA E GUARIGIONE
    public StructPerRequisitoMalattiaGuarigione valoriMalattiaOGuarigione;
    // VALORI PERCENTUALE CORRUZIONE
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
    
    public StructPerQuantitaMutazioni valoriQuantitaMutazioni;

}
public enum TipoRequisito
{
    MalattiaOGuarigione,
    PercentualeCorruzione,
    NumeroPoteriOttenuti,
    PoterSpecifico,
    NumeroStanzeAttraversate,
    DistanzaPercorsa,
    ValoreDiStatistica,
    MutazioniAccumulate,
    Flag
}
public enum RequisitiGuarigioneMalattia
{
    Guarigione,
    Malattia
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


[System.Serializable]
public struct StructPerRequisitoMalattiaGuarigione
{
    public RequisitiGuarigioneMalattia tipoRequisitoGuarigioneMalattia;
    public OperatoriDiComparamento operatori;
    public float valoreDaComparare;
}
[System.Serializable]
public struct StructPerPercentualeCorruzione
{
    public OperatoriDiComparamento operatori;
    public float valoreDaComparare;
}
[System.Serializable]
public struct StructPerTipiPotereStanza
{
    public ListaTipiPoteriPerRequisiti listaTipiPoteriPerRequisiti;
    public OperatoriDiComparamento operatori;
    public int valore;
}
[System.Serializable]
public struct StructPerPoteriSpecificiStanza
{
    [Searchable]
    public ListaTuttiPoteri listaTipiPoteriPerRequisiti;
    public bool hasPotere;
}
[System.Serializable]
public struct StructPerEntrataStanze
{
    public OperatoriDiComparamento operatori;
    public int valore;
}
[System.Serializable]
public struct StructPerConteggioDistanzaPercorsa
{
    public OperatoriDiComparamento operatori;
    public int valore;
}
[System.Serializable]
public struct StructPerValoreStatistica
{
    public OperatoriDiComparamento operatori;
    public int valoreStatistica;
}
[System.Serializable]
public struct StructPerQuantitaMutazioni
{
    public OperatoriDiComparamento operatori;
    public int valoreQuantitaMutazione;
}