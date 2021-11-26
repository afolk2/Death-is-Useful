using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles Aim of the Necromancer/Minion
public class SkeletonAim : AIAim
{
    private SkeletonKit kit;
    void Start()
    {
        kit = GetComponentInChildren<SkeletonKit>();
    }
    public override void SetAimTarget(Vector2 mousePos)
    {
        base.SetAimTarget(mousePos);
        kit.ItemAim(aimTarget, mousePos.x < transform.position.x);
    }
}
