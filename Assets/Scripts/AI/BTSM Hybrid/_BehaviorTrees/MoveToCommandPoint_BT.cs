using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToCommandPoint_BT : BehaviorTree
{
    Transform target;
    public override void StartTree(BTFiniteStateMachine sm)
    {
        base.StartTree(sm);
        Blackboard.Add("DestinationSetter", GetComponent<Pathfinding.AIDestinationSetter>());
        Blackboard.Add("Transform", transform);
        target = MinionManager.manager.GetTarget();
        mRoot = new BTRepeator(this, new BTParallel(this, new BTNode[] { new BTMoveToTarget(this, target), new BTAnimateMovement(this)}));
    }

    public override void ExitTree()
    {
        Debug.Log("No longer moving to command point");
    }
}
