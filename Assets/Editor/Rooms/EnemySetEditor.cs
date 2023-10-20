using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemySet))]
public class EnemySetEditor : Editor
{
    private GUIStyle styleLabel;
    private SerializedProperty tipoSwitchOndataProp;
    private SerializedProperty pesoSetProp;
    private SerializedProperty tipoRequisitoProp;
    private SerializedProperty valoreCorruzioneProp;
    private SerializedProperty valoriPerPoteriProp;
    private SerializedProperty valoriPerPoteriSpecificiProp;
    private SerializedProperty valoriDistanzaPercorsaProp;
    private SerializedProperty valoriPerEntrataStanzeProp;
    private SerializedProperty valoriStatistichePersonaggioProp;
    private SerializedProperty listaDiNemiciDaSpawnareInStanzaProp;
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

        tipoSwitchOndataProp = serializedObject.FindProperty("tipoSwitchOndata");
        pesoSetProp = serializedObject.FindProperty("pesoSet");
        tipoRequisitoProp = serializedObject.FindProperty("tipoRequisito");
        valoreCorruzioneProp = serializedObject.FindProperty("valoreCorruzione");
        valoriPerPoteriProp = serializedObject.FindProperty("valoriPerPoteri");
        valoriPerPoteriSpecificiProp = serializedObject.FindProperty("valoriPerPoteriSpecifici");
        valoriDistanzaPercorsaProp = serializedObject.FindProperty("valoriDistanzaPercorsa");
        valoriPerEntrataStanzeProp = serializedObject.FindProperty("valoriPerEntrataStanze");
        valoriStatistichePersonaggioProp = serializedObject.FindProperty("valoriStatistichePersonaggio");
        listaDiNemiciDaSpawnareInStanzaProp = serializedObject.FindProperty("listaDiNemiciDaSpawnareInStanza");
        flagOnSelectProp = serializedObject.FindProperty("flagOnSelect");
        flagOnCompleteProp = serializedObject.FindProperty("flagOnComplete");
        canRepeatProp = serializedObject.FindProperty("CanRepeat");
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
                break;

            case TipoRequisito.NumeroPoteriOttenuti:
                EditorGUILayout.PropertyField(valoriPerPoteriProp, new GUIContent("Valori per Poteri"));
                break;

            case TipoRequisito.PoterSpecifico:
                EditorGUILayout.PropertyField(valoriPerPoteriSpecificiProp, new GUIContent("Valori per Poteri Specifici"));
                break;

            case TipoRequisito.DistanzaPercorsa:
                EditorGUILayout.PropertyField(valoriDistanzaPercorsaProp, new GUIContent("Valori Distanza Percorsa"));
                break;

            case TipoRequisito.NumeroStanzeAttraversate:
                EditorGUILayout.PropertyField(valoriPerEntrataStanzeProp, new GUIContent("Valori per Entrata Stanze"));
                break;

            case TipoRequisito.ValoreDiStatistica:
                EditorGUILayout.PropertyField(valoriStatistichePersonaggioProp, new GUIContent("Valori Statistica Personaggio"));
                break;

            default:
                break;
        }

        EditorGUILayout.PropertyField(listaDiNemiciDaSpawnareInStanzaProp, new GUIContent("Lista di Nemici da Spawnare in Stanza"), true);
        EditorGUILayout.PropertyField(flagOnSelectProp, new GUIContent("Flags On Select"), true);
        EditorGUILayout.PropertyField(flagOnCompleteProp, new GUIContent("Flags On Complete"), true);
        EditorGUILayout.PropertyField(canRepeatProp, new GUIContent("Can Repeat"));

        serializedObject.ApplyModifiedProperties();
    }
}
