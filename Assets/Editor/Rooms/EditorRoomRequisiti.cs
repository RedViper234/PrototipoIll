using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RequisitiStanza))]
public class EditorRoomRequisiti : Editor
{
    private GUIStyle styleLabel;
    private SerializedProperty tipoRequisitoProp;
    private SerializedProperty valoriCorruzioneProp;
    private SerializedProperty valoriPerPoteriProp;
    private SerializedProperty valoriPerPoteriSpecificiProp;
    private SerializedProperty valoriDistanzaPercorsa;
    private SerializedProperty valoriPerEntrataStanze;
    private SerializedProperty valoriStatistichePersonaggio;
    // Add other SerializedProperty variables for your other fields

    private void OnEnable()
    {
        styleLabel = new GUIStyle();
        styleLabel.richText = true;
        styleLabel.alignment = TextAnchor.MiddleCenter;
        styleLabel.margin = new RectOffset(0, 0, 0, 20);
        styleLabel.normal.textColor = Color.white;

        tipoRequisitoProp = serializedObject.FindProperty("tipoRequisito");
        valoriDistanzaPercorsa = serializedObject.FindProperty("valoriDistanzaPercorsa");
        valoriCorruzioneProp = serializedObject.FindProperty("valoreCorruzione");
        valoriPerPoteriProp = serializedObject.FindProperty("valoriPerPoteri");
        valoriPerPoteriSpecificiProp = serializedObject.FindProperty("valoriPerPoteriSpecifici");
        valoriPerEntrataStanze = serializedObject.FindProperty("valoriPerEntrataStanze");
        valoriStatistichePersonaggio = serializedObject.FindProperty("valoriStatistichePersonaggio");
        // Initialize other SerializedProperty variables here
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUILayout.Label("<size=20><b><color=#ffffffff>Tipologia Requisiti</color></b></size>", styleLabel);

        RequisitiStanza requisiti = (RequisitiStanza)target;

        EditorGUILayout.PropertyField(tipoRequisitoProp, new GUIContent("Tipo di Requisito"));

        switch ((TipoRequisito)tipoRequisitoProp.enumValueIndex)
        {
            case TipoRequisito.PercentualeCorruzione:
                EditorGUILayout.Space(10);
                EditorGUILayout.PropertyField(valoriCorruzioneProp.FindPropertyRelative("operatori"), new GUIContent("Operatore"));
                EditorGUILayout.PropertyField(valoriCorruzioneProp.FindPropertyRelative("valoreDaComparare"), new GUIContent("Valore da Comparare"));
                break;

            case TipoRequisito.NumeroPoteriOttenuti:
                EditorGUILayout.Space(10);
                EditorGUILayout.PropertyField(valoriPerPoteriProp.FindPropertyRelative("listaTipiPoteriPerRequisiti"), new GUIContent("Tipo di Potere"));
                EditorGUILayout.PropertyField(valoriPerPoteriProp.FindPropertyRelative("operatori"), new GUIContent("Operatore"));
                EditorGUILayout.PropertyField(valoriPerPoteriProp.FindPropertyRelative("valore"), new GUIContent("Valore"));
                break;

            case TipoRequisito.PoterSpecifico:
                EditorGUILayout.Space(10);
                EditorGUILayout.PropertyField(valoriPerPoteriSpecificiProp.FindPropertyRelative("listaTipiPoteriPerRequisiti"), new GUIContent("Potere Specifico"));
                EditorGUILayout.PropertyField(valoriPerPoteriSpecificiProp.FindPropertyRelative("hasPotere"), new GUIContent("Ha il Potere"));
                break;
            case TipoRequisito.DistanzaPercorsa:
                EditorGUILayout.Space(10);
                EditorGUILayout.PropertyField(valoriDistanzaPercorsa.FindPropertyRelative("operatori"), new GUIContent("Operatore"));
                EditorGUILayout.PropertyField(valoriDistanzaPercorsa.FindPropertyRelative("valore"), new GUIContent("Valore"));
                break;
            case TipoRequisito.NumeroStanzeAttraversate:
                EditorGUILayout.Space(10);
                EditorGUILayout.PropertyField(valoriPerEntrataStanze.FindPropertyRelative("operatori"), new GUIContent("Operatore"));
                EditorGUILayout.PropertyField(valoriPerEntrataStanze.FindPropertyRelative("valore"), new GUIContent("Valore"));
                break;   
            case TipoRequisito.ValoreDiStatistica:
                EditorGUILayout.Space(10);
                EditorGUILayout.PropertyField(valoriStatistichePersonaggio.FindPropertyRelative("operatori"), new GUIContent("Operatore"));
                EditorGUILayout.PropertyField(valoriStatistichePersonaggio.FindPropertyRelative("valoreStatistica"), new GUIContent("Valore"));
                break;
            // Add other cases for your other fields

            default:
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
