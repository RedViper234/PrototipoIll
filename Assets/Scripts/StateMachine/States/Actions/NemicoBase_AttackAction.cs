using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Nemici/Actions/NemicoBase_AttackAction", menuName = "Nemici/Actions/NemicoBase_AttackAction")]
public class NemicoBase_AttackAction : Action
{
    [SerializeField] private float attackRadius;
    [SerializeField] private LayerMask attackMask;
    public override void ActOnEntryState(StateMachineController controller)
    {
        controller.StartCoroutine(IniziaAttacco(controller));
    }
    public override void Act(StateMachineController controller)
    {
        return;
    }

    private IEnumerator IniziaAttacco(StateMachineController controller)
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            Debug.Log("Sto Attaccando");
            RaycastHit2D playerHit = Physics2D.CircleCast(controller.gameObject.transform.position, attackRadius, Vector2.zero, 0, attackMask);
            if(playerHit.collider != null)
            {
                Damageable damageable = playerHit.collider?.GetComponent<Damageable>();
                damageable.TakeDamage(10);
            }
        }

    }

    public override void ActOnExitState(StateMachineController controller)
    {
        return;
    }

   
}
