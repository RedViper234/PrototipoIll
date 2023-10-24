using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FlagManager))]
public class FlagManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        FlagManager componente = (FlagManager)target;

        // Visualizza la lista nell'editor
        serializedObject.Update();
        SerializedProperty trueFlagsList = serializedObject.FindProperty("playerTrueFlags");
        SerializedProperty falseFlagsList = serializedObject.FindProperty("playerFalseFlags");
        EditorGUILayout.PropertyField(trueFlagsList, true);
        EditorGUILayout.PropertyField(falseFlagsList, true);

        // Applica le modifiche alla lista
        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Imposta flagName uguale al nome"))
        {
            for (int i = 0; i < trueFlagsList.arraySize; i++)
            {
                SerializedProperty flagElement = trueFlagsList.GetArrayElementAtIndex(i);
                SerializedProperty flagProperty = flagElement.FindPropertyRelative("flag");
                SerializedProperty flagNameProperty = flagElement.FindPropertyRelative("flagName");
                FlagsSO flagSO = flagProperty.objectReferenceValue as FlagsSO;

                if (flagSO != null)
                {
                    flagNameProperty.stringValue = flagSO.nomeFlag;
                }
            }

            for (int i = 0; i < falseFlagsList.arraySize; i++)
            {
                SerializedProperty flagElement = falseFlagsList.GetArrayElementAtIndex(i);
                SerializedProperty flagProperty = flagElement.FindPropertyRelative("flag");
                SerializedProperty flagNameProperty = flagElement.FindPropertyRelative("flagName");
                FlagsSO flagSO = flagProperty.objectReferenceValue as FlagsSO;

                if (flagSO != null)
                {
                    flagNameProperty.stringValue = flagSO.nomeFlag;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}