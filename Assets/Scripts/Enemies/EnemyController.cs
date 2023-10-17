using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(StateMachineController))]
public class EnemyController : MonoBehaviour
{

    [SerializeField] private EnemyStatsSO m_statisticheNemico;
    private StateMachineController enemyStateMachineController;
    public bool isNotAttacking = true;
    public Transform target;

    [HideInInspector] public NavMeshAgent currentAgent;
    [HideInInspector] public EnemyStatsSO enemyStats;
    [HideInInspector] public Damageable damageable;
    [HideInInspector] public Animator animatorNemico;
    public EnemyStatsSO EnemyStats
    {
        get { return m_statisticheNemico; }
    }
    // Start is called before the first frame update
    private void Awake()
    {
        damageable = GetComponent<Damageable>();
        currentAgent = GetComponent<NavMeshAgent>();
        enemyStateMachineController = GetComponent<StateMachineController>();
        enemyStats = EnemyStats;
        animatorNemico = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        target = FindFirstObjectByType<PlayerController>().transform;
        currentAgent.acceleration = enemyStats.enemyAcceleration;
        currentAgent.speed = enemyStats.enemySpeed;
        damageable.maxHealth = enemyStats.vitaMassima;
    }
    public void SetUpAI()
    {
        currentAgent.enabled = enemyStateMachineController.aiAttiva;
    }
    // Update is called once per frame
    void Update()
    {
        if (enemyStateMachineController.aiAttiva)
        {
            currentAgent.isStopped = !enemyStateMachineController.aiAttiva;
        }
        else
        {
            currentAgent.isStopped = !enemyStateMachineController.aiAttiva;
        }
        CheckForAnimator();
    }

    private void CheckForAnimator()
    {
        Vector3 rbVelocity = currentAgent.velocity.normalized;
        animatorNemico.SetFloat("Dir_x", rbVelocity.x);
        animatorNemico.SetFloat("Dir_y", rbVelocity.y);
    }

    private void OnDestroy()
    {
        if (!this.gameObject.scene.isLoaded) return;
        if (AppManager.Instance.enemyManager != null)
            AppManager.Instance.enemyManager?.RemoveEnemyFromScene(this);
    }


}
