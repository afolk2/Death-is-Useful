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
    }
    #region Input Handle Methods

    private void HandleMove(Vector2 input)
    {
        skeletonInput.moveInput = input;
    }

    private void HandlePrimaryAction(float input)
    {
        if (input > 0)
            skeletonInput.MainPress();
        else
            skeletonInput.MainRelease();
    }
    private void HandleSecondaryAction(float input)
    {
        if (input > 0)
            skeletonInput.OffPress();
        else
            skeletonInput.OffRelease();
    }
    private void HandleAim(Vector2 input)
    {
        skeletonInput.mousePositionInput = mainCamera.ScreenToWorldPoint(input);
    }

    private void HandleInteractAction()
    {
        skeletonInput.Interact();
    }
    #endregion

    private void Awake()
    {
        controls = new DIUInputs();
    }
    #endregion
    [SerializeField] private Camera mainCamera;
    private NecromancerInput skeletonInput;
    private void Start()
    {
        skeletonInput = GetComponent<NecromancerInput>();
        if (mainCamera == null)
            mainCamera = Camera.main;
    }


}
