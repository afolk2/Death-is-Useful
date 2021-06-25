using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordShield : Kit
{
    [Header("-- Sword Attack Stats --")]
    [SerializeField] private int damage;
    [SerializeField] private float reach;
    [SerializeField] private float swingTime;

    [SerializeField] private LayerMask hitMask;

    [Header("-- Shield Block Stats --")]
    [SerializeField] private int blockStrength;
    [SerializeField] private float blockArcHeight, blockArcWidth;
    [SerializeField] private float counterTime;
    [SerializeField] private float shieldStretch;

    private LineRenderer arcRend;

    private bool backSlash;
    private bool blocking;
    public override void MainAction(EquipmentManager m)
    {
        if (m.skeletonKit.idle && !blocking)
        {
            m.skeletonKit.SimpleMelee(damage, reach, swingTime, mainHand, hitMask, backSlash, true);
            backSlash = !backSlash;
        }

    }

    public override void MainRelease(EquipmentManager m)
    {
        // This Item does not have a release action
    }

    public override void SecondaryAction(EquipmentManager m)
    {
        if (m.skeletonKit.idle || !blocking)
        {
            blocking = true;
            m.skeletonKit.Block(blockStrength, blockArcWidth, blockArcHeight, counterTime, shieldStretch, mainHand, offHand);
        }
            
    }

    public override void SecondaryRelease(EquipmentManager m)
    {
        blocking = false;
        m.skeletonKit.StopBlock(mainHand, offHand);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, reach);
    }

#endif

}
