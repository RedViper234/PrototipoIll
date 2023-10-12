using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

/// <summary>
/// Custom editor for <see cref="EditorRoomData"/> objects.
/// </summary>
[CustomEditor(typeof(RoomData))]
public class EditorRoomData : Editor
{

    public SerializedProperty requisitiStanza,priorita, isFirstRoom, raritaEValoreStanza;
    GUIStyle styleLabel;

    /// <summary>
    /// Called when this editor is loaded.
    /// </summary>
    private void OnEnable()
    {
        requisitiStanza = serializedObject.FindProperty("requisitiStanza");
        priorita = serializedObject.FindProperty("prioritaStanza");
        isFirstRoom = serializedObject.FindProperty("isFirstRoom");
        raritaEValoreStanza = serializedObject.FindProperty("raritaEValoreStanza");
        styleLabel = new GUIStyle();
        styleLabel.richText = true;
        styleLabel.alignment = TextAnchor.MiddleCenter;
        styleLabel.margin = new RectOffset(0, 0, 0, 20);
    }




    /// <summary>
    /// Draws the GUI for the edited object(s).
    /// </summary>
    public override void OnInspectorGUI()
    {
        RoomData roomData = (RoomData)target;
        GUILayout.Label("<size=20><b><color=#ffffffff>Tipologia Stanze</color></b></size>", styleLabel);

        // Mostra il campo per la scelta del tipo di stanza
        roomData.tipiDiStanza = (TipiDiStanza)EditorGUILayout.EnumFlagsField("Tipo di Stanza", roomData.tipiDiStanza);

        // Mostra solo le enum relative alle sottocategorie selezionate
        EditorGUILayout.LabelField("Sottocategorie:");

        EditorGUI.indentLevel++;

        if ((roomData.tipiDiStanza & TipiDiStanza.Combattimento) != 0)
        {
            roomData.tipoStanzaCombattimento = (SottoCategoriaStanzaCombattimento)EditorGUILayout.EnumPopup("Combattimento", roomData.tipoStanzaCombattimento);
        }

        if ((roomData.tipiDiStanza & TipiDiStanza.Boss) != 0)
        {
            roomData.tipoStanzaBoss = (SottoCategoriaStanzaBoss)EditorGUILayout.EnumPopup("Boss", roomData.tipoStanzaBoss);
        }

        if ((roomData.tipiDiStanza & TipiDiStanza.Evento) != 0)
        {
            roomData.tipoStanzaEvento = (SottoCategoriaStanzaEvento)EditorGUILayout.EnumPopup("Evento", roomData.tipoStanzaEvento);
        }

        if ((roomData.tipiDiStanza & TipiDiStanza.Storia) != 0)
        {
            roomData.tipoStanzaStoria = (SottoCategoriaStanzaStoria)EditorGUILayout.EnumPopup("Storia", roomData.tipoStanzaStoria);
        }

        EditorGUI.indentLevel--;
        if ((roomData.tipiDiStanza & TipiDiStanza.Combattimento) != 0 || (roomData.tipiDiStanza & TipiDiStanza.Boss) != 0)
        {
            roomData.setDiMostriDellaStanza = (MonsterSet)EditorGUILayout.ObjectField("Set di Mostri", roomData.setDiMostriDellaStanza, typeof(MonsterSet), false);
        }
        DrawUILine(Color.gray,1);
        EditorGUILayout.Space(5);

        GUILayout.Label("<size=20><b><color=#ffffffff>Lista Vequisiti</color></b></size>", styleLabel);
        EditorGUILayout.PropertyField(requisitiStanza, new GUIContent("Lista requisiti"), true);
        DrawUILine(Color.gray, 1);
        EditorGUILayout.Space(5);
        GUILayout.Label("<size=20><b><color=#ffffffff>Altre Variabili</color></b></size>", styleLabel);
        EditorGUILayout.PropertyField(priorita, new GUIContent("Priorità"), true);
        EditorGUILayout.PropertyField(isFirstRoom, new GUIContent("È la prima stanza?"), true);
        EditorGUILayout.PropertyField(raritaEValoreStanza, new GUIContent("Rarità e valore:"), true);
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
        // Mostra il set di mostri della stanza
    }
    public static void DrawUILine(Color color, int thickness = 2, int padding = 10)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        r.y += padding / 2;
        r.x -= 2;
        r.width += 6;
        EditorGUI.DrawRect(r, color);
    }


}
