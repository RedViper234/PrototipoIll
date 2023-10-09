using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName ="Nemici/StatBaseNemico",menuName = "Nemici/StatBaseNemico")]
public class EnemyStatsSO : ScriptableObject
{
    [Header("Stat Base Nemico")]
    public int vitaMassima;
    public float currentHealth;
    [Space] 
    public float enemySpeed;
    public float enemyAcceleration;
    [Space]
    [Header("Resistenze"),Range(0,100)]
    public float resistenzaKnockback;
    public float resistenzaAlDannoSottrattiva = 0;
    [Range(0,100)]
    public float resistenzaAlDannoPercentuale = 0;
    [Space]
    [Header("Attacchi nemico")]
    public List<EnemyAttackSO> listaDiAttacchiNemico;

    [Space(20)]
    [Header("Eventi base nemico")]
    public UnityEvent OnAttack;
    public UnityEvent OnPlayerDamaged;
    public UnityEvent OnEnemyDamaged;


    public virtual void ApplicaDannoAlGiocatore()
    {
        OnPlayerDamaged?.Invoke();
    }




}
