//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(RoomDistribution))]
//public class RoomDistributionEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        RoomDistribution roomDistribution = (RoomDistribution)target;

//        // Disegna il campo per il tipo di stanza
//        roomDistribution.tipoStanzaArea = (TipiDiStanza)EditorGUILayout.EnumPopup("Tipo di Stanza", roomDistribution.tipoStanzaArea);

//        // A seconda del tipo di stanza, mostra le sottocategorie pertinenti
//        switch (roomDistribution.tipoStanzaArea)
//        {
//            case TipiDiStanza.Combattimento:
//                roomDistribution.tipoStanzaCombattimentoArea = (SottoCategoriaStanzaCombattimento)EditorGUILayout.EnumPopup("Sottocategoria Combattimento", roomDistribution.tipoStanzaCombattimentoArea);

//                break;
//            case TipiDiStanza.Boss:
//                roomDistribution.tipoStanzaBossArea = (SottoCategoriaStanzaBoss)EditorGUILayout.EnumPopup("Sottocategoria Boss", roomDistribution.tipoStanzaBossArea);
//                break;
//            case TipiDiStanza.Evento:
//                roomDistribution.tipoStanzaEvento = (SottoCategoriaStanzaEvento)EditorGUILayout.EnumPopup("Sottocategoria Evento", roomDistribution.tipoStanzaEvento);
//                break;
//            case TipiDiStanza.Storia:
//                //roomDistribution.tipoStanzaStoriaArea = (SottoCategoriaStanzaStoria)EditorGUILayout.EnumPopup("Sottocategoria Storia", roomDistribution.tipoStanzaStoriaArea, typeof(SottoCategoriaStanzaStoria), true);
//                roomDistribution.tipoStanzaStoriaArea = (SottoCategoriaStanzaStoria)EditorGUILayout.EnumPopup(new GUIContent("Sottocategoria Storia"), roomDistribution.tipoStanzaStoriaArea);
//                break;
//            default:
//                break;
//        }

//        // Applica le modifiche all'oggetto target
//        serializedObject.ApplyModifiedProperties();
//    }
//}
