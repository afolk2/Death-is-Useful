using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds all input variables and can be adjusted by either and AIController or PlayerController.
/// </summary>
public class NecromancerInput : MonoBehaviour
{
    [Header("Input Variables")]
    public Vector2 moveInput;
    public Vector2 mousePositionInput;

    private PlayerMovement movement;
    private SkeletonAim skeletonAim;
    private EquipmentManager equipment;
    private MinionManager minionManager;
    [SerializeField] LayerMask interactLayer;
    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
        skeletonAim = GetComponent<SkeletonAim>();
        equipment = GetComponentInChildren<EquipmentManager>();
        minionManager = MinionManager.manager;
    }

    private void Update()
    {
        movement.Move(moveInput);
        skeletonAim.SetAimTarget(mousePositionInput);
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

    public void Interact()
    {
        Collider2D c = Physics2D.OverlapPoint(transform.position, interactLayer);
        SummonableSkeleton summon = c.GetComponentInParent<SummonableSkeleton>();
        if (summon != null)
        {
            if (minionManager.CheckSummonCost(summon.cost))
            {
                summon.Spawn();
                minionManager.ChangePower(-summon.cost);
            }
        }
    }
    public void MakeCommand(int commandIndex, Vector2 mousePositionInput)
    {
        if (Vector2.Distance(mousePositionInput, transform.position) < 2)
            minionManager.FollowPlayer();
        else
            minionManager.MoveHere(commandIndex, mousePositionInput);
    }
}
