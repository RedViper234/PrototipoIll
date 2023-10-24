using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
using System.Collections.Generic;

/// <summary>
/// Custom editor for <see cref="EditorRoomData"/> objects.
/// </summary>
[CustomEditor(typeof(RoomData))]
public class EditorRoomData : Editor
{

    public SerializedProperty prefabStanza, setDiMostriDellaStanza, requisitiStanza,priorita, isFirstRoom, raritaEValoreStanza, flagsOnEnter, flagsOnComplete;
    GUIStyle styleLabel;
    public GUIStyle greenBackgroundStyle;
    public GUIStyle redBackgroundStyle;

    /// <summary>
    /// Called when this editor is loaded.
    /// </summary>
    private void OnEnable()
    {
        setDiMostriDellaStanza = serializedObject.FindProperty("setDiMostriDellaStanza");
        prefabStanza = serializedObject.FindProperty("prefabStanza");
        requisitiStanza = serializedObject.FindProperty("requisitiStanza");
        priorita = serializedObject.FindProperty("prioritaStanza");
        isFirstRoom = serializedObject.FindProperty("isFirstRoom");
        raritaEValoreStanza = serializedObject.FindProperty("raritaEValoreStanza");
        flagsOnEnter = serializedObject.FindProperty("flagsOnEnter");
        flagsOnComplete = serializedObject.FindProperty("flagsOnComplete");
        styleLabel = new GUIStyle();
        styleLabel.richText = true;
        styleLabel.alignment = TextAnchor.MiddleCenter;
        styleLabel.margin = new RectOffset(0, 0, 0, 20);
        greenBackgroundStyle = new GUIStyle();
        greenBackgroundStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/preubenbar.png") as Texture2D;
        redBackgroundStyle = new GUIStyle();
        redBackgroundStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/preubenbar.png") as Texture2D;
        redBackgroundStyle.normal.textColor = Color.white;
        greenBackgroundStyle.normal.textColor = Color.white;
    }




    /// <summary>
    /// Draws the GUI for the edited object(s).
    /// </summary>
    public override void OnInspectorGUI()
    {
        GUILayout.Label("<size=30><b><color=#00ffffff>Stanza</color></b></size>", styleLabel);
        EditorGUILayout.PropertyField(prefabStanza, new GUIContent("Prefab Stanza"), true);
        RoomData roomData = (RoomData)target;
        GUILayout.Space(10);
        DrawUILine(Color.gray, 1);
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
            switch (roomData.tipoStanzaEvento)
            {
                case SottoCategoriaStanzaEvento.Mercante:
                    roomData.sottoTipoStanzaMercante = (ETipoEventiMercante)EditorGUILayout.EnumPopup("Tipo Di Mercante", roomData.sottoTipoStanzaMercante);
                    break;
                case SottoCategoriaStanzaEvento.Guaritori:
                    break;
                case SottoCategoriaStanzaEvento.StanzaRiposo:
                    roomData.sottoTipoStanzaRiposo = (ETipoEventiStanzaRiposo)EditorGUILayout.EnumPopup("Tipo Di Riposo", roomData.sottoTipoStanzaRiposo);
                    break;
                case SottoCategoriaStanzaEvento.Carovana:
                    break;
                default:
                    break;
            }
        }

        if ((roomData.tipiDiStanza & TipiDiStanza.Storia) != 0)
        {
            roomData.tipoStanzaStoria = (SottoCategoriaStanzaStoria)EditorGUILayout.EnumPopup("Storia", roomData.tipoStanzaStoria);
        }

        EditorGUI.indentLevel--;
        if ((roomData.tipiDiStanza & TipiDiStanza.Combattimento) != 0 || (roomData.tipiDiStanza & TipiDiStanza.Boss) != 0)
        {
            EditorGUILayout.PropertyField(setDiMostriDellaStanza,new GUIContent("Lista set mostri"));
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
        DrawUILine(Color.gray, 1);
        GUILayout.Label("<size=20><b><color=#ffffffff>Flags</color></b></size>", styleLabel);
        EditorGUILayout.PropertyField(flagsOnEnter, new GUIContent("Flags On Enter"), true);
        EditorGUILayout.PropertyField(flagsOnComplete, new GUIContent("Flags On Complete"), true);

        // Draw flagsDaSettare and flagsNecessarie with custom styles
        
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

    private Texture2D MakeTex(int width, int height, Color color)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++)
        {
            pix[i] = color;
        }

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();

        return result;
    }
}
