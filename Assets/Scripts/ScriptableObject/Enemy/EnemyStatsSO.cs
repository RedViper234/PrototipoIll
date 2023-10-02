using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName ="Nemici/StatBaseNemico",menuName = "Nemici/StatBaseNemico")]
public class EnemyStatsSO : ScriptableObject
{
    [Header("Stat Base Nemico")]
    public float vitaMassima;
    public float currentHealth;
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