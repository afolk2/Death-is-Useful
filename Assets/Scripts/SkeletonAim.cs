using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles Aim of the Necromancer/Minion
public class SkeletonAim : MonoBehaviour
{
    private SkeletonKit kit;
    // Start is called before the first frame update
    void Start()
    { 
        kit = GetComponentInChildren<SkeletonKit>();
    }
    public void Aim(Vector2 mousePos)
    {
        kit.ItemAim(mousePos - new Vector2(transform.position.x, transform.position.y), mousePos.x < transform.position.x);
    }
}
