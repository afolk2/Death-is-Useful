using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTMoveToPlayer : BTNode
{
    private AIDestinationSetter destSetter;
    private AIPath path;
    protected Transform Destination { get; set; }
    public float speed;
    private float minDistance;
    public BTMoveToPlayer(BehaviorTree t) : base(t)
    {
        minDistance = MinionManager.settings.minimumFollowDistance;

        //GET PLAYER TRANSFORM
        object o;
        bool found = Tree.Blackboard.TryGetValue("PlayerTransform", out o);

        if (found)
        {
            Destination = (Transform)o;
        }
        else
        {
            Debug.LogWarning(Tree.transform.name + "Could not access player position, manually getting from singleton (FIX ME)");
            Destination = MinionManager.settings.GetPlayer();
        }

        //GET AIDESTINATIONSETTER AND AIPATH
        found = Tree.Blackboard.TryGetValue("DestinationSetter", out o); //Note: replaced o with new object could probably make this more readable
        if (found)
        {
            destSetter = (AIDestinationSetter)o;
            path = destSetter.transform.GetComponent<AIPath>();
            destSetter.target = Destination;
        }
        else
        {
            Debug.LogWarning(Tree.transform.name + " Did not have an AIDestinationSetter attached inside BTMoveToPlayer Node. Manually placing one (Fix this tho)");
            destSetter = Tree.gameObject.AddComponent<AIDestinationSetter>();
            path = Tree.gameObject.AddComponent<AIPath>();
        }
    }

    public override Result Execute()
    {
        //if we've arrived at random position
        if (path.remainingDistance < minDistance)
        {
            return Result.Success;
        }
        else
        {
            if (path.canMove && path.canSearch && Destination != null)
                return Result.Running;
            else
            {
                Debug.Log("Cant Find path or Move");
                return Result.Failure;
            }
        }
    }
}
