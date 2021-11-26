using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behavior Tree State Machine Hybrid. AI Code base
/// </summary>
public class AI_BTSM : MonoBehaviour
{
    public BTFiniteStateMachine aiSystem;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        aiSystem = new BTFiniteStateMachine(this);
        aiSystem.ChangeTree<MinionFollowPlayer_BT>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        aiSystem.UpdateTree();
    }

    public void RemoveTree(BehaviorTree tree)
    {
        Destroy(tree);
    }
}
