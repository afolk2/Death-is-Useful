using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer_BT : BehaviorTree
{
    public override void StartTree(BTFiniteStateMachine sm)
    {
        base.StartTree(sm);

        Blackboard.Add("FollowTransform", MinionManager.manager.GetPlayerMoveGuide());
        Blackboard.Add("DestinationSetter", GetComponent<Pathfinding.AIDestinationSetter>());
        Blackboard.Add("Path", GetComponent<Pathfinding.AIPath>());
        Blackboard.Add("Transform", transform);
        mRoot = new BTRepeator(this, new BTParallel(this, new BTNode[] { new BTMoveToPlayer(this), new BTAnimateMovement(this)}));
    }

    public override void ExitTree()
    {
        Debug.Log("No longer following player");
    }
}
