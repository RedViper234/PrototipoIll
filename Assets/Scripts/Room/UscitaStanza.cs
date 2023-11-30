using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UscitaStanza : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            switch (GameManager.GetCurrentGameState())
            {
                case GameStates.FineCombattimento:
                    AppManager.Instance.enemyManager.RemoveEveryEnemyFromTheList();
                    AppManager.Instance.roomManager.VaiAlPuntoSuccessivo();
                    break;
                case GameStates.Combattimento:
                    Debug.Log("Combattimento");
                    break;
                case GameStates.CombattimentoBoss:
                    Debug.Log("CombattimentoBoss");
                    break;
                case GameStates.FineEvento:
                    AppManager.Instance.enemyManager.RemoveEveryEnemyFromTheList();
                    AppManager.Instance.roomManager.VaiAlPuntoSuccessivo();
                    break;
                default:
                    break;
            }

            
        }
    }
}
