using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponInputTest : MonoBehaviour
{
    private PlayerInput inputActions;
    private InputAction attackAction;
    public GameObject weaponPrefab;

    void Start()
    {
        inputActions = new PlayerInput();
        
        attackAction = inputActions.Player.Meeleattack;
        attackAction.performed += Attack;
        // attackAction.canceled += StopAttack;
        attackAction.Enable();
    }

    private void Attack(InputAction.CallbackContext obj)
    {
        weaponPrefab.GetComponent<Weapon>().ExecuteCombo();
    }

    private void StopAttack(InputAction.CallbackContext obj)
    {
        weaponPrefab.GetComponent<Weapon>().StopCombo();
    }

    void OnDisable()
    {
        attackAction.performed -= Attack;
    }
}
