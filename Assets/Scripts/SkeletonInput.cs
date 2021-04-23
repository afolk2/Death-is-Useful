using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds all input variables and can be adjusted by either and AIController or PlayerController.
/// </summary>
public class SkeletonInput : MonoBehaviour
{
    [Header("Input Variables")]
    public Vector2 moveInput;
    public Vector2 mousePositionInput;
    public float actionOneInput, actionTwoInput;

    private SkeletonMovement movement;
    private SkeletonAim skeletonAim;

    private void Start()
    {
        movement = GetComponent<SkeletonMovement>();
        skeletonAim = GetComponent<SkeletonAim>();
    }

    private void Update()
    {
        movement.Move(moveInput);
        skeletonAim.Aim(mousePositionInput);
    }

}
