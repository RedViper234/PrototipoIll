using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemySet))]
public class EnemySetEditor : Editor
{
    private GUIStyle styleLabel;
    private SerializedProperty tipologiaNemico;
    private SerializedProperty tipoSwitchOndataProp;
    private SerializedProperty pesoSetProp;
    private SerializedProperty tipoRequisitoProp;
    private SerializedProperty valoreCorruzioneProp;
    private SerializedProperty valoriPerPoteriProp;
    private SerializedProperty valoriPerPoteriSpecificiProp;
    private SerializedProperty valoriDistanzaPercorsaProp;
    private SerializedProperty valoriPerEntrataStanzeProp;
    private SerializedProperty valoriStatistichePersonaggioProp;
    private SerializedProperty listaDiOndate;
    private SerializedProperty valoreVitaRimasta;
    private SerializedProperty valoreQuantitaMassima;
    private SerializedProperty valoreQuantitaUstioni;
    private SerializedProperty flagOnSelectProp;
    private SerializedProperty flagOnCompleteProp;
    private SerializedProperty canRepeatProp;

    private void OnEnable()
    {
        styleLabel = new GUIStyle();
        styleLabel.richText = true;
        styleLabel.alignment = TextAnchor.MiddleCenter;
        styleLabel.margin = new RectOffset(0, 0, 0, 20);
        styleLabel.normal.textColor = Color.white;

        tipologiaNemico = serializedObject.FindProperty("tipologiaNemico");
        tipoSwitchOndataProp = serializedObject.FindProperty("tipoSwitchOndata");
        pesoSetProp = serializedObject.FindProperty("pesoSet");
        tipoRequisitoProp = serializedObject.FindProperty("tipoRequisito");
        valoreCorruzioneProp = serializedObject.FindProperty("valoreCorruzione");
        valoriPerPoteriProp = serializedObject.FindProperty("valoriPerPoteri");
        valoriPerPoteriSpecificiProp = serializedObject.FindProperty("valoriPerPoteriSpecifici");
        valoriDistanzaPercorsaProp = serializedObject.FindProperty("valoriDistanzaPercorsa");
        valoriPerEntrataStanzeProp = serializedObject.FindProperty("valoriPerEntrataStanze");
        valoriStatistichePersonaggioProp = serializedObject.FindProperty("valoriStatistichePersonaggio");
        listaDiOndate = serializedObject.FindProperty("listaDiOndate");
        flagOnSelectProp = serializedObject.FindProperty("flagsOnSelect");
        flagOnCompleteProp = serializedObject.FindProperty("flagsOnComplete");
        canRepeatProp = serializedObject.FindProperty("CanRepeat");
        valoreVitaRimasta = serializedObject.FindProperty("valoreVitaRimasta");
        valoreQuantitaMassima = serializedObject.FindProperty("valoreQuantitaMassima");
        valoreQuantitaUstioni = serializedObject.FindProperty("valoreQuantitaUstioni");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUILayout.Label("<size=20><b><color=#ffffffff>Enemy Sets</color></b></size>", styleLabel);

        EnemySet enemySet = (EnemySet)target;

        EditorGUILayout.PropertyField(tipoSwitchOndataProp, new GUIContent("Tipo Switch Ondata"));
        EditorGUILayout.PropertyField(pesoSetProp, new GUIContent("Peso Set"));
        EditorGUILayout.PropertyField(tipoRequisitoProp, new GUIContent("Tipo di Requisito"));

        // Add the switch statement to display and edit the specific fields for TipoRequisito
        switch ((TipoRequisito)tipoRequisitoProp.enumValueIndex)
        {
            case TipoRequisito.PercentualeCorruzione:
                EditorGUILayout.PropertyField(valoreCorruzioneProp, new GUIContent("Valore Corruzione"));
                EditorGUILayout.Space(20);
                break;

            case TipoRequisito.NumeroPoteriOttenuti:
                EditorGUILayout.PropertyField(valoriPerPoteriProp, new GUIContent("Valori per Poteri"));
                EditorGUILayout.Space(20);
                break;

            case TipoRequisito.PoterSpecifico:
                EditorGUILayout.PropertyField(valoriPerPoteriSpecificiProp, new GUIContent("Valori per Poteri Specifici"));
                EditorGUILayout.Space(20);
                break;

            case TipoRequisito.DistanzaPercorsa:
                EditorGUILayout.PropertyField(valoriDistanzaPercorsaProp, new GUIContent("Valori Distanza Percorsa"));
                EditorGUILayout.Space(20);
                break;

            case TipoRequisito.NumeroStanzeAttraversate:
                EditorGUILayout.PropertyField(valoriPerEntrataStanzeProp, new GUIContent("Valori per Entrata Stanze"));
                EditorGUILayout.Space(20);
                break;

            case TipoRequisito.ValoreDiStatistica:
                EditorGUILayout.PropertyField(valoriStatistichePersonaggioProp, new GUIContent("Valori Statistica Personaggio"));
                EditorGUILayout.Space(20);
                break;
            case TipoRequisito.QuantitaVitaMassima:
                EditorGUILayout.PropertyField(valoreQuantitaMassima, new GUIContent("Valori Quantità Vita Massima"));
                EditorGUILayout.Space(20);
                break;
            case TipoRequisito.PercentualeVitaRimasta:
                EditorGUILayout.PropertyField(valoreVitaRimasta, new GUIContent("Valori Vita Rimasta"));
                EditorGUILayout.Space(20);
                break;
            case TipoRequisito.Ustioni:
                EditorGUILayout.PropertyField(valoreQuantitaUstioni, new GUIContent("Valori Quantita Ustioni"));
                EditorGUILayout.Space(20);
                break;
            default:
                break;
        }

        EditorGUILayout.PropertyField(tipologiaNemico, new GUIContent("Tipologia Mostri"));
        EditorGUILayout.PropertyField(listaDiOndate, new GUIContent("Lista di ondate"), true);
        EditorGUILayout.PropertyField(flagOnSelectProp, new GUIContent("Flags On Select"), true);
        EditorGUILayout.PropertyField(flagOnCompleteProp, new GUIContent("Flags On Complete"), true);
        EditorGUILayout.PropertyField(canRepeatProp, new GUIContent("Can Repeat"));

        serializedObject.ApplyModifiedProperties();

        //base.OnInspectorGUI();
    }
}
