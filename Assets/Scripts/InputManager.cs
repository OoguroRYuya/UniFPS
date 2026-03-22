using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;
    private PlayerMotor motor;
    private PlayerLook look;
    private RaycastGun gun;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake(){
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        gun = GetComponent<RaycastGun>();

        onFoot.Jump.performed +=  ctx => motor.Jump();
        onFoot.Crouch.performed += ctx => motor.Crouch();
        onFoot.Sprint.performed += ctx => motor.Sprint();
        onFoot.Shoot.started += ctx => gun.StartShooting();
        onFoot.Shoot.canceled += ctx => gun.StopShooting();
    } 

    // Update is called once per frame
    void FixedUpdate(){
        //tell the PlayerMotor to move using the value from our movement action
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    void OnEnable(){
        onFoot.Enable();
    }

    void OnDisable(){
        onFoot.Disable(); 
    }

}
