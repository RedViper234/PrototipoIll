using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrag: MonoBehaviour
{
    public Coroutine playerDragCoroutine;

    void Awake()
    {
        EventManager.HandleOnEnemyHit += StopDrag;
    }

    public IEnumerator DraggingPlayer(AWeapon weaponReference, PlayerDragStruct playerDrag, Vector2 ActualDirection)
    {
        var wpController = weaponReference.transform.parent.GetComponent<WeaponController>();
       
        var playerCtrl = wpController.playerController;
       
        var playerRb = playerCtrl.GetComponent<Rigidbody2D>();

        playerCtrl.ManageMovement(false);
        
        var initPos = playerCtrl.transform.position;
        
        Vector2 dragDirection = ChooseDirection(weaponReference, playerDrag, ActualDirection);

        var dragForce = dragDirection.normalized * playerDrag.force;
        var startTime = Time.time;
        var endTime = startTime + playerDrag.duration;

        while (Time.time < endTime)
        {
            playerRb.AddForce(dragForce, ForceMode2D.Force);
            yield return new WaitForFixedUpdate();
        }

        playerRb.velocity = Vector2.zero;

        var finalPos = playerCtrl.transform.position;

        var distance = Vector2.Distance(initPos, finalPos);
        
        playerCtrl.ManageMovement(true);

        Destroy(this.gameObject);
    }

    public void StopDrag()
    {
        if(playerDragCoroutine != null)
        {
            StopCoroutine(playerDragCoroutine);
            playerDragCoroutine = null;
        }
    }

    private Vector2 ChooseDirection(AWeapon weaponReference, PlayerDragStruct playerDrag, Vector2 ActualDirection)
    {
        switch(playerDrag.dragDirection)
        {
            case DragDirection.WeaponDefault:
                return weaponReference.playerDrag.direction;
            case DragDirection.Up:
                return Vector2.up;
            case DragDirection.Down:
                return Vector2.down;
            case DragDirection.Left:
                return Vector2.left;
            case DragDirection.Right:
                return Vector2.right;
            case DragDirection.UpLeft:
                return Vector2.up + Vector2.left;
            case DragDirection.UpRight:
                return Vector2.up + Vector2.right;
            case DragDirection.DownLeft:
                return Vector2.down + Vector2.left;
            case DragDirection.DownRight:
                return Vector2.down + Vector2.right;
            case DragDirection.Forward:
                return ActualDirection;
            case DragDirection.Backward:
                return -ActualDirection;
            default:
                return ActualDirection;
        }
    }

    public void StartDragging(AWeapon weapon, PlayerDragStruct dragData, Vector2 direction)
    {
        playerDragCoroutine = StartCoroutine(DraggingPlayer(weapon, dragData, direction));
    }

    void OnDestroy()
    {
        EventManager.HandleOnEnemyHit -= StopDrag;
    }
}

    //OLD
    // protected IEnumerator DraggingPlayer()
    // {
    //     var wpController = weaponReference.transform.parent.GetComponent<WeaponController>();
       
    //     var playerCtrl = wpController.playerController;
       
    //     var playerRb = playerCtrl.GetComponent<Rigidbody2D>();

    //     playerCtrl.ManageMovement(false);
        
    //     var initPos = playerCtrl.transform.position;
        
    //     ChooseDirection();

    //     var dragForce = playerDrag.direction.normalized * playerDrag.force;
    //     var startTime = Time.time;
    //     var endTime = startTime + playerDrag.duration;

    //     while (Time.time < endTime)
    //     {
    //         playerRb.AddForce(dragForce, ForceMode2D.Force);
    //         yield return new WaitForFixedUpdate();
    //     }

    //     playerRb.velocity = Vector2.zero;

    //     var finalPos = playerCtrl.transform.position;

    //     var distance = Vector2.Distance(initPos, finalPos);

    //     // UnityEngine.Debug.Log($"Player Distance Run: {Math.Round(distance, 2)} in direction: {playerDrag.direction}");
        
    //     playerCtrl.ManageMovement(true);
    // }

    // private void ChooseDirection()
    // {
    //     switch(playerDrag.dragDirection)
    //     {
    //         case DragDirection.WeaponDefault:
    //             playerDrag.direction = weaponReference.playerDrag.direction;
    //             break;
    //         case DragDirection.Up:
    //             playerDrag.direction = Vector2.up;
    //             break;
    //         case DragDirection.Down:
    //             playerDrag.direction = Vector2.down;
    //             break;
    //         case DragDirection.Left:
    //             playerDrag.direction = Vector2.left;
    //             break;
    //         case DragDirection.Right:
    //             playerDrag.direction = Vector2.right;
    //             break;
    //         case DragDirection.UpLeft:
    //             playerDrag.direction = Vector2.up + Vector2.left;
    //             break;
    //         case DragDirection.UpRight:
    //             playerDrag.direction = Vector2.up + Vector2.right;
    //             break;
    //         case DragDirection.DownLeft:
    //             playerDrag.direction = Vector2.down + Vector2.left;
    //             break;
    //         case DragDirection.DownRight:
    //             playerDrag.direction = Vector2.down + Vector2.right;
    //             break;
    //         case DragDirection.Forward:
    //             playerDrag.direction = ActualDirection;
    //             break;
    //         case DragDirection.Backward:
    //             playerDrag.direction = -ActualDirection;
    //             break;
    //         default:
    //             playerDrag.direction = ActualDirection;
    //             break;
    //     }
    // }
