using UnityEngine;
using UnityEditor;


/// <summary>
/// Custom editor for <see cref="EditorRoomManager"/> objects.
/// </summary>
[CustomEditor(typeof(RoomManager))]
public class EditorRoomManager : Editor
{
    private void OnEnable()
    {
        
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        RoomManager manager = (RoomManager)target;
        EditorGUILayout.Space(20);

        if(GUILayout.Button(new GUIContent("Avanti nell'ondata"), EditorStyles.miniButton)){
            manager.VaiAvantiDiOndata();
        }
        EditorGUILayout.Space(5);
        if(GUILayout.Button(new GUIContent("Indietro nell'ondata"), EditorStyles.miniButton)){
            manager.VaiIndietroOndate();
        }

    }


}
