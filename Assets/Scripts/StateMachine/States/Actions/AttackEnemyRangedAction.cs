using System.Collections;
using UnityEngine;


/// <summary>
/// 
/// </summary>
[CreateAssetMenu(fileName = "Nemici/Actions/AttackEnemyRangedAction", menuName = "Nemici/Actions/AttackEnemyRangedAction")]
public class AttackEnemyRangedAction : Action
{
    public GameObject bulletPrefab;
    float bulletSpeed = 10f;

    [SerializeField]
    private float shootingDelay = 0;
    [SerializeField,Tooltip("Errore tempo di ripresa:\nè la modifica massima/minima del tempo di ripresa dello sparo, esempio: 2 sec con errore 0.25 vuol dire che fra uno sparo e l’altro passa fra gli 1.75 sec e i 2.25 sec")]
    private float errorAttesaSparo = 0;
    public override void Act(StateMachineController controller)
    {
        return;
    }

    public override void ActionDrawGizmos(StateMachineController controller)
    {
        return;
    }

    public override void ActOnEntryState(StateMachineController controller)
    {
        controller.StartCoroutine(ShootDelay(controller));
    }

    public override void ActOnExitState(StateMachineController controller)
    {
        return;
    }




    public IEnumerator ShootDelay(StateMachineController controller)
    {
        yield return new WaitForSeconds(shootingDelay);
        controller.StartCoroutine(ShootRoutine(controller));
    }
    private IEnumerator ShootRoutine(StateMachineController controller)
    {
        while (true)
        {
            float randomDelay = shootingDelay + Random.Range(-errorAttesaSparo, errorAttesaSparo);
            yield return new WaitForSeconds(randomDelay);
            Shoot(controller);
        }
    }
    private void Shoot(StateMachineController controller)
    {

        Vector2 directionToPlayer = (controller.currentEnemy.target.position - controller.currentEnemy.transform.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, controller.currentEnemy.transform.position, controller.currentEnemy.transform.rotation);
        Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
        bulletRigidbody.velocity = directionToPlayer * bulletSpeed;
        Destroy(bullet, 10f);
    }
}
