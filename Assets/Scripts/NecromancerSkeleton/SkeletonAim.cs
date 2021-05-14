using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles Aim of the Necromancer/Minion
public class SkeletonAim : MonoBehaviour
{
    private SkeletonKit kit;
    private Vector2 aim;
    public Vector2 Aim
    {
        get
        {
            return aim;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        kit = GetComponentInChildren<SkeletonKit>();
    }
    public void DoAim(Vector2 mousePos)
    {
        aim = mousePos - new Vector2(transform.position.x, transform.position.y);
        kit.ItemAim(aim, mousePos.x < transform.position.x);
    }
}
