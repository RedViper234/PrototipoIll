using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PuntoDiInteresseComponent : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descrizione;


    public void SetDescrizione(string descrizione)
    {
        this.descrizione.text = descrizione;
    }
}
