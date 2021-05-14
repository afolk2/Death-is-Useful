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

    private SkeletonMovement movement;
    private SkeletonAim skeletonAim;
    private EquipmentManager equipment;

    private void Start()
    {
        movement = GetComponent<SkeletonMovement>();
        skeletonAim = GetComponent<SkeletonAim>();
        equipment = GetComponentInChildren<EquipmentManager>();
    }

    private void Update()
    {
        movement.Move(moveInput);
        skeletonAim.DoAim(mousePositionInput);
    }

    public void MainPress()
    {
        equipment.UseMain();
    }

    public void MainRelease()
    {
        equipment.ReleaseMain();
    }

    public void OffPress()
    {
        equipment.UseOff();
    }

    public void OffRelease()
    {
        equipment.ReleaseOff();
    }

}
