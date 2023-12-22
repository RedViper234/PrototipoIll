using UnityEngine;
using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;


public struct CostruzionePuntiDiInteressi : IMessage {
    public Dictionary<PuntoDiInteresse,StrutturaPerDictionaryRoom>  dizionarioPuntiEStanze;
    public PuntoDiInteresse puntoDiInteresseChiave;
    public CostruzionePuntiDiInteressi(Dictionary<PuntoDiInteresse,StrutturaPerDictionaryRoom> dizionarioPuntiEStanze, PuntoDiInteresse puntoDiInteresseChiave)
    {
        this.dizionarioPuntiEStanze = dizionarioPuntiEStanze;
        this.puntoDiInteresseChiave = puntoDiInteresseChiave;
    }
}