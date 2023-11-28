using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    private PlayerInput inputActions;
    private InputAction attackAction;
    private InputAction specialAttackAction;
    public GameObject weaponPrefab;
    public PlayerController playerController;

    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
        
        inputActions = new PlayerInput();
        
        //Attack action assignment
        attackAction = inputActions.Player.Meeleattack;
        attackAction.performed += Attack;
        attackAction.Enable();

        //special Attack action assignment
        specialAttackAction = inputActions.Player.SpecialAttack;
        specialAttackAction.performed += SpecialAttack;
        specialAttackAction.Enable();
    }

    private void Attack(InputAction.CallbackContext obj)
    {
        weaponPrefab.GetComponent<AWeapon>().ExecuteCombo();
    }

    private void SpecialAttack(InputAction.CallbackContext obj)
    {
        weaponPrefab.GetComponent<AWeapon>().ExecuteSpecialAttack();
    }

    private void StopAttack(InputAction.CallbackContext obj)
    {
        weaponPrefab.GetComponent<AWeapon>().StopCombo();
    }

    void OnDisable()
    {
        attackAction.performed -= Attack;
    }
}
