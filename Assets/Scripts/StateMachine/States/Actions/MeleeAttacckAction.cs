using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Nemici/Actions/MeleeAttacckAction", menuName = "Nemici/Actions/MeleeAttacckAction")]
public class MeleeAttacckAction : Action
{
    [SerializeField] private float attackRadius;
    [SerializeField] private LayerMask attackMask;
    
    private Coroutine coroutine;
    public override void ActOnEntryState(StateMachineController controller)
    {
        Debug.Log("ENTRATO IN STATO ATTACC0");
        if (controller.currentEnemy.isNotAttacking)
        {
            controller.StartCoroutine(IniziaAttacco(controller));
        }
    }
    public override void Act(StateMachineController controller)
    {
        return;
    }

    private IEnumerator IniziaAttacco(StateMachineController controller)
    {
        while (true)
        {
            controller.currentEnemy.isNotAttacking = false;
            yield return new WaitForSeconds(3f);
            RaycastHit2D playerHit = Physics2D.CircleCast(controller.gameObject.transform.position, attackRadius, Vector2.zero, 0, attackMask);
            if(playerHit.collider != null)
            {
                Debug.Log("Sto Attaccando");
                Damageable damageable = playerHit.collider?.GetComponent<Damageable>();
                damageable.TakeDamage(10);
                
            }
            controller.currentEnemy.isNotAttacking = true;
        }

    }

    public override void ActOnExitState(StateMachineController controller)
    {
        controller.StopCoroutine(IniziaAttacco(controller));
    }

    public override void ActionDrawGizmos(StateMachineController controller)
    {
        return;
    }
}
