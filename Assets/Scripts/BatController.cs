using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class BatController : MonoBehaviour
{

    AIStateMachine sm;



    void Start()
    {
        sm = new AIStateMachine();
        sm.ChangeState(new EnemyChaseClosestTarget(this));
    }

    // Update is called once per frame
    void Update()
    {
        sm.Update();
    }


    
}
