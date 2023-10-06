using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(StateMachineController))]
public class EnemyController : MonoBehaviour
{

    [SerializeField] private EnemyStatsSO m_statisticheNemico;
    private StateMachineController enemyStateMachineController;
    public bool isNotAttacking = true;
    public EnemyStatsSO EnemyStats
    {
        get { return m_statisticheNemico; }
    }
    // Start is called before the first frame update
    private void Awake()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
    }
}
