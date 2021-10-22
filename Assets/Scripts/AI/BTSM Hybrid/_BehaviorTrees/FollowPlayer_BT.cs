using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer_BT : BehaviorTree
{
    public FollowPlayer_BT()
    {

    }

    public override void StartTree(BTFiniteStateMachine sm)
    {
        base.StartTree(sm);

        Blackboard.Add("PlayerTransform", MinionManager.settings.GetPlayer());
        Blackboard.Add("DestinationSetter", GetComponent<Pathfinding.AIDestinationSetter>());
        mRoot = new BTRepeator(this, new BTSequencer(this, new BTNode[] { new BTMoveToPlayer(this)}));
    }

    public override void ExitTree()
    {
        Debug.Log("No longer following player");
    }
}
