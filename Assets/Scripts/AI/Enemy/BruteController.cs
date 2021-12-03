using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BruteController : AI_BTSM
{
    protected override void Start()
    {
        aiSystem = new BTFiniteStateMachine(this);
        aiSystem.ChangeTree<EnemyPatrol_BT>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        aiSystem.UpdateTree();
    }
}
