using UnityEngine;
using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;


public struct CostruzionePuntiDiInteressi : IMessage {
    public SerializedDictionary<PuntoDiInteresse,RoomData>  dizionarioPuntiEStanze;
    public PuntoDiInteresse puntoDiInteresseChiave;
    public CostruzionePuntiDiInteressi(SerializedDictionary<PuntoDiInteresse,RoomData> dizionarioPuntiEStanze, PuntoDiInteresse puntoDiInteresseChiave)
    {
        this.dizionarioPuntiEStanze = dizionarioPuntiEStanze;
        this.puntoDiInteresseChiave = puntoDiInteresseChiave;
    }
}