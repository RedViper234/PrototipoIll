using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FlagsSO))]
public class FlagEditor : Editor
{
    public override void OnInspectorGUI()
    {
        FlagsSO flag = (FlagsSO)target;

        // Visualizza il nome corrente nell'editor
        EditorGUILayout.LabelField("Nome Corrente:", flag.nomeFlag);

        // Se si desidera, puoi consentire all'utente di modificarlo manualmente
        flag.nomeFlag = EditorGUILayout.TextField("Nome", flag.nomeFlag);
        flag.valoreBaseFlag = EditorGUILayout.Toggle("Valore di Base", flag.valoreBaseFlag);
        // Imposta il nome dello ScriptableObject sul suo nome di risorsa (senza estensione)
        string assetPath = AssetDatabase.GetAssetPath(target);
        string assetName = System.IO.Path.GetFileNameWithoutExtension(assetPath);
        flag.nomeFlag = assetName;

        // Applica le modifiche all'oggetto ScriptableObject
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
        }
    }
}

