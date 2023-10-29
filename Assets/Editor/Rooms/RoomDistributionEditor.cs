//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(StaticRoomDistribution))]
//public class RoomDistributionEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        StaticRoomDistribution roomDistribution = (StaticRoomDistribution)target;

//        // Disegna il campo per il tipo di stanza
//        roomDistribution.tipoStanzaArea = (TipiDiStanzaFLag)EditorGUILayout.EnumPopup("Tipo di Stanza", roomDistribution.tipoStanzaArea);

//        // A seconda del tipo di stanza, mostra le sottocategorie pertinenti
//        switch (roomDistribution.tipoStanzaArea)
//        {
//            case TipiDiStanzaFLag.Combattimento:
//                roomDistribution.combattimento = (SottoCategoriaStanzaCombattimento)EditorGUILayout.EnumPopup("Sottocategoria Combattimento", roomDistribution.combattimento);

//                break;
//            case TipiDiStanzaFLag.Boss:
//                roomDistribution.boss = (SottoCategoriaStanzaBoss)EditorGUILayout.EnumPopup("Sottocategoria Boss", roomDistribution.boss);
//                break;
//            case TipiDiStanzaFLag.Evento:
//                roomDistribution.evento = (SottoCategoriaStanzaEvento)EditorGUILayout.EnumPopup("Sottocategoria Evento", roomDistribution.evento);
//                break;
//            case TipiDiStanzaFLag.Storia:
//                //roomDistribution.storia = (SottoCategoriaStanzaStoria)EditorGUILayout.EnumPopup("Sottocategoria Storia", roomDistribution.storia, typeof(SottoCategoriaStanzaStoria), true);
//                roomDistribution.storia = (SottoCategoriaStanzaStoria)EditorGUILayout.EnumPopup(new GUIContent("Sottocategoria Storia"), roomDistribution.storia);
//                break;
//            default:
//                break;
//        }

//        // Applica le modifiche all'oggetto target
//        serializedObject.ApplyModifiedProperties();
//    }
//}
