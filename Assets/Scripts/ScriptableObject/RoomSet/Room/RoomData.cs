using System;
using UnityEngine;


/// <summary>
/// 
/// </summary>
[CreateAssetMenu(fileName = "Rooms/RoomData", menuName = "Rooms/RoomData")]
public class RoomData : ScriptableObject
{
    [SerializeField] private DifficoltaStanza m_difficoltaStanza; 
    [SerializeField] private TipiDiStanza m_tipiDiStanza;
    [Space(30)]
    public MonsterSet setDiMostriDellaStanza;
}
public enum DifficoltaStanza
{
    Facile,
    Media,
    Difficile
}
[Flags]
public enum TipiDiStanza
{
    None = 0,
    Combattimento,
    Boss,
    Evento,
    Storia

}