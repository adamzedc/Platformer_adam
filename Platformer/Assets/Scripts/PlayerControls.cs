using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{

    public PlayerInputActions playerControls;

    public InputAction move;
    public InputAction jump;
    public InputAction look;
    public InputAction crouch;
    public InputAction slide;
    public InputAction sprint;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

        jump = playerControls.Player.Jump;
        jump.Enable();

        look = playerControls.Player.Look;
        look.Enable();

        crouch = playerControls.Player.Crouch;
        crouch.Enable();

        slide = playerControls.Player.Slide;
        slide.Enable();

        sprint = playerControls.Player.Sprint;
        sprint.Enable();
        
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        look.Disable();
        crouch.Disable();
        slide.Disable();
        sprint.Disable();
    }
}
