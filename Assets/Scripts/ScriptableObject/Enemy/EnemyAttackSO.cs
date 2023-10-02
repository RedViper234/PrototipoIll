using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="Nemici/Attacchi",menuName = "Nemici/Attacchi")]
public class EnemyAttackSO : ScriptableObject
{
    public TipoAttacco tipoAttacco = TipoAttacco.Base;
    public float dannoAttacco;
    [Tooltip("Ogni danno presenta una variazione esprimibile in percentuale o in valore che va a modificare ogni volta l’effettivo danno inflitto. Esempio: una variazione del 10% su un danno di 10 provoca un danno che oscilla fra 9 e 11, mentre una variazione di 2 su un danno di 10 può causare fra gli 8 e 12 danni. I due tipi di variazione sono alternativi l’uno all’altro.")]
    [Range(0, 10)] public int variazioneDiDanno;
    [Range(0, 10)] public float knockBackForce;

}
public enum TipoAttacco
{
    Base,
    AttaccoMischia,
    AttaccoDistanza
}