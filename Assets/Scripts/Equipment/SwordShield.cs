using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordShield : Kit
{
    [Header("-- Sword Stats --")]
    [SerializeField] private int damage;
    [SerializeField] private float arc;
    [SerializeField] private float reach;
    [SerializeField] private float swingTime;
    [SerializeField] private float anticipationTime;
    [SerializeField] private float recoveryTime;
    [SerializeField] private int blockStrength;

    private bool backSlash;
    public override void MainAction(EquipmentManager m)
    {
        if(m.skeletonKit.idle)
        {
            m.skeletonKit.StartMeleeAttack(m.skeletonAim.Aim, arc, reach, anticipationTime, swingTime, recoveryTime, backSlash, true);
            backSlash = !backSlash;
        }
            
    }

    public override void MainRelease(EquipmentManager m)
    {
        
    }

    public override void SecondaryAction(EquipmentManager m)
    {
        //Switch hand and held item to block sprite (sideways shield)

        //Emit a block area around player (rounded shield collider)

        //Reduce block health while this action is held and hit. (maybe need ui to show how many block the player has remaining)
    }

    public override void SecondaryRelease(EquipmentManager m)
    {
        
    }
}
