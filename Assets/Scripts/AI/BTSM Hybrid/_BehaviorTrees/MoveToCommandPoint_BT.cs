using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToCommandPoint_BT : BehaviorTree
{
    Transform target;
    public MoveToCommandPoint_BT(Transform target)
    {
        this.target = target;
    }
    public override void StartTree(BTFiniteStateMachine sm)
    {
        base.StartTree(sm);
        Blackboard.Add("DestinationSetter", GetComponent<Pathfinding.AIDestinationSetter>());
        mRoot = new BTRepeator(this, new BTSequencer(this, new BTNode[] { new BTMoveToTarget(this, target)}));
    }

    public override void ExitTree()
    {
        Debug.Log("No longer following player");
    }
}
