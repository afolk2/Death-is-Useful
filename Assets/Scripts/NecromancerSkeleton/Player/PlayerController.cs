using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Handles Player inputs. Uses new Unity Input System
    /// </summary>
    #region Setup Input
    private DIUInputs controls;
    private void OnEnable()
    {
        controls.Enable();
        controls.Player.Enable();

        controls.Player.Move.performed += ctx => HandleMove(ctx.ReadValue<Vector2>());
        controls.Player.Move.Enable();

        controls.Player.Aim.performed += ctx => HandleAim(ctx.ReadValue<Vector2>());
        controls.Player.Aim.Enable();

        controls.Player.PrimaryAction.performed += ctx => HandlePrimaryAction(ctx.ReadValue<float>());
        controls.Player.PrimaryAction.canceled += ctx => HandlePrimaryAction(ctx.ReadValue<float>());
        controls.Player.PrimaryAction.Enable();

        controls.Player.SecondaryAction.performed += ctx => HandleSecondaryAction(ctx.ReadValue<float>());
        controls.Player.SecondaryAction.canceled += ctx => HandleSecondaryAction(ctx.ReadValue<float>());
        controls.Player.SecondaryAction.Enable();

        controls.Player.Interact.started += ctx => HandleInteractAction();
        controls.Player.Interact.Enable();

        controls.Player.CommandOne.performed += ctx => HandleCommandAction(ctx.ReadValue<float>(), 0);
        controls.Player.CommandOne.Enable();

        controls.Player.CommandTwo.performed += ctx => HandleCommandAction(ctx.ReadValue<float>(), 1);
        controls.Player.CommandTwo.Enable();

        controls.Player.CommandThree.performed += ctx => HandleCommandAction(ctx.ReadValue<float>(), 2);
        controls.Player.CommandThree.Enable();

        controls.Player.CommandFour.performed += ctx => HandleCommandAction(ctx.ReadValue<float>(), 3);
        controls.Player.CommandFour.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
        controls.Player.Disable();

        controls.Player.Move.performed -= ctx => HandleMove(ctx.ReadValue<Vector2>());
        controls.Player.Move.Disable();

        controls.Player.Aim.performed -= ctx => HandleAim(ctx.ReadValue<Vector2>());
        controls.Player.Aim.Disable();

        controls.Player.PrimaryAction.performed -= ctx => HandlePrimaryAction(ctx.ReadValue<float>());
        controls.Player.PrimaryAction.canceled -= ctx => HandlePrimaryAction(ctx.ReadValue<float>());
        controls.Player.PrimaryAction.Disable();

        controls.Player.SecondaryAction.performed -= ctx => HandleSecondaryAction(ctx.ReadValue<float>());
        controls.Player.SecondaryAction.canceled -= ctx => HandleSecondaryAction(ctx.ReadValue<float>());
        controls.Player.SecondaryAction.Disable();

        controls.Player.Interact.started -= ctx => HandleInteractAction();
        controls.Player.Interact.Disable();

        controls.Player.CommandOne.performed -= ctx => HandleCommandAction(ctx.ReadValue<float>(), 0);
        controls.Player.CommandOne.Disable();

        controls.Player.CommandTwo.performed -= ctx => HandleCommandAction(ctx.ReadValue<float>(), 1);
        controls.Player.CommandTwo.Disable();

        controls.Player.CommandThree.performed -= ctx => HandleCommandAction(ctx.ReadValue<float>(), 2);
        controls.Player.CommandThree.Disable();

        controls.Player.CommandFour.performed -= ctx => HandleCommandAction(ctx.ReadValue<float>(), 3);
        controls.Player.CommandFour.Disable();
    }
    #region Input Handle Methods

    private void HandleMove(Vector2 input)
    {
        necroInput.moveInput = input;
    }

    private void HandlePrimaryAction(float input)
    {
        if (input > 0)
            necroInput.MainPress();
        else
            necroInput.MainRelease();
    }
    private void HandleSecondaryAction(float input)
    {
        if (input > 0)
            necroInput.OffPress();
        else
            necroInput.OffRelease();
    }
    private void HandleAim(Vector2 input)
    {
        necroInput.mousePositionInput = mainCamera.ScreenToWorldPoint(input);
    }

    private void HandleInteractAction()
    {
        necroInput.Interact();
    }

    private void HandleCommandAction(float input, int commandIndex)
    {
        if(input > 0)
            necroInput.MakeCommand(commandIndex, necroInput.mousePositionInput);
    }
    #endregion

    private void Awake()
    {
        controls = new DIUInputs();
    }

    private void FixedUpdate()
    {
        guide.UpdateGuides(necroInput.moveInput, necroInput.mousePositionInput);
    }
    #endregion
    [SerializeField] private Camera mainCamera;
    private NecromancerInput necroInput;
    private MoveGuide guide;
    private void Start()
    {
        guide = GetComponent<MoveGuide>();
        //TODO set value to match foot radius;
        guide.SetupGuide(.3f);
        necroInput = GetComponent<NecromancerInput>();
        if (mainCamera == null)
            mainCamera = Camera.main;
    }


}
