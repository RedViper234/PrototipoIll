using UnityEngine;
using UnityEditor;


/// <summary>
/// Custom editor for <see cref="EditorRoomRequisiti"/> objects.
/// </summary>
[CustomEditor(typeof(RequisitiStanza))]
public class EditorRoomRequisiti : Editor
{
    GUIStyle styleLabel;


    /// <summary>
    /// Called when this editor is loaded.
    /// </summary>
    private void OnEnable()
    {
        styleLabel = new GUIStyle();
        styleLabel.richText = true;
        styleLabel.alignment = TextAnchor.MiddleCenter;
        styleLabel.margin = new RectOffset(0, 0, 0, 20);
        styleLabel.normal.textColor = Color.white;
    }

    /// <summary>
    /// Draws the GUI for the edited object(s).
    /// </summary>
    public override void OnInspectorGUI()
    {
        GUILayout.Label("<size=20><b><color=#ffffffff>Tipologia Requisiti</color></b></size>", styleLabel);
        RequisitiStanza requisiti = (RequisitiStanza)target;
        
        // Mostra il campo per il tipo di requisito
        requisiti.tipoRequisito = (TipoRequisito)EditorGUILayout.EnumPopup("Tipo di Requisito", requisiti.tipoRequisito);

        // In base al tipo di requisito selezionato, mostra le opzioni corrispondenti
        switch (requisiti.tipoRequisito)
        {
            case TipoRequisito.MalattiaOGuarigione:
                EditorGUILayout.Space(10);
                requisiti.valoriMalattiaOGuarigione.tipoRequisitoGuarigioneMalattia = (RequisitiGuarigioneMalattia)EditorGUILayout.EnumPopup("Guarigione o Malattia", requisiti.valoriMalattiaOGuarigione.tipoRequisitoGuarigioneMalattia);
                requisiti.valoriMalattiaOGuarigione.operatori = (OperatoriDiComparamento)EditorGUILayout.EnumPopup("Operatore", requisiti.valoriMalattiaOGuarigione.operatori);
                requisiti.valoriMalattiaOGuarigione.valoreDaComparare = EditorGUILayout.FloatField("Valore da Comparare", requisiti.valoriMalattiaOGuarigione.valoreDaComparare);
                break;

            case TipoRequisito.PercentualeCorruzione:
                EditorGUILayout.Space(10);
                requisiti.valoreCorruzione.operatori = (OperatoriDiComparamento)EditorGUILayout.EnumPopup("Operatore", requisiti.valoreCorruzione.operatori);
                requisiti.valoreCorruzione.valoreDaComparare = EditorGUILayout.FloatField("Valore da Comparare", requisiti.valoreCorruzione.valoreDaComparare);
                break;

            case TipoRequisito.NumeroPoteriOttenuti:
                EditorGUILayout.Space(10);
                requisiti.valoriPerPoteri.listaTipiPoteriPerRequisiti = (ListaTipiPoteriPerRequisiti)EditorGUILayout.EnumPopup("Tipo di Potere", requisiti.valoriPerPoteri.listaTipiPoteriPerRequisiti);
                requisiti.valoriPerPoteri.operatori = (OperatoriDiComparamento)EditorGUILayout.EnumPopup("Operatore", requisiti.valoriPerPoteri.operatori);
                requisiti.valoriPerPoteri.valore = EditorGUILayout.IntField("Valore", requisiti.valoriPerPoteri.valore);
                break;

            case TipoRequisito.PoterSpecifico:
                EditorGUILayout.Space(10);
                requisiti.valoriPerPoteriSpecifici.listaTipiPoteriPerRequisiti = (ListaTuttiPoteri)EditorGUILayout.EnumPopup("Potere Specifico", requisiti.valoriPerPoteriSpecifici.listaTipiPoteriPerRequisiti);
                requisiti.valoriPerPoteriSpecifici.hasPotere = EditorGUILayout.Toggle("Ha il Potere", requisiti.valoriPerPoteriSpecifici.hasPotere);
                break;
            case TipoRequisito.NumeroStanzeAttraversate:
                EditorGUILayout.Space(10);
                requisiti.valoriPerEntrataStanze.operatori = (OperatoriDiComparamento)EditorGUILayout.EnumPopup("Operatore", requisiti.valoriPerEntrataStanze.operatori);
                requisiti.valoriPerEntrataStanze.valore = EditorGUILayout.IntField("Valore", requisiti.valoriPerEntrataStanze.valore);
                break;
            case TipoRequisito.DistanzaPercorsa:
                EditorGUILayout.Space(10);
                requisiti.valoriDistanzaPercorsa.operatori = (OperatoriDiComparamento)EditorGUILayout.EnumPopup("Operatore", requisiti.valoriDistanzaPercorsa.operatori);
                requisiti.valoriDistanzaPercorsa.valore = EditorGUILayout.IntField("Valore", requisiti.valoriDistanzaPercorsa.valore);
                break;
            case TipoRequisito.ValoreDiStatistica:
                EditorGUILayout.Space(10);
                requisiti.valoriStatistichePersonaggio.operatori = (OperatoriDiComparamento)EditorGUILayout.EnumPopup("Operatore", requisiti.valoriStatistichePersonaggio.operatori);
                requisiti.valoriStatistichePersonaggio.valoreStatistica = EditorGUILayout.IntField("Valore", requisiti.valoriStatistichePersonaggio.valoreStatistica);
                break;
            case TipoRequisito.MutazioniAccumulate:
                EditorGUILayout.Space(10);
                requisiti.valoriQuantitaMutazioni.operatori = (OperatoriDiComparamento)EditorGUILayout.EnumPopup("Operatore", requisiti.valoriQuantitaMutazioni.operatori);
                requisiti.valoriQuantitaMutazioni.valoreQuantitaMutazione = EditorGUILayout.IntField("Valore", requisiti.valoriQuantitaMutazioni.valoreQuantitaMutazione);
                break;
            // Aggiungi altre casistiche qui

            default:
                break;
        }
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }
}
