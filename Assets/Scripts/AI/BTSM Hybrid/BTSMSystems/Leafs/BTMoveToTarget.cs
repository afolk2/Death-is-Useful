using Pathfinding;
using UnityEngine;

public class BTMoveToTarget : BTNode
{
    private AIDestinationSetter destSetter;
    private AIPath path;
    protected Transform Destination { get; set; }
    public float speed;
    public BTMoveToTarget(BehaviorTree t, Transform target) : base(t)
    {
        Destination = target;
        object o;
        bool found = Tree.Blackboard.TryGetValue("DestinationSetter", out o);

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
        if (path.reachedDestination)
        {
            return Result.Success;
        }
        else
        {
            if (path.canMove && path.canSearch && path.hasPath)
                return Result.Running;
            else
            {
                Debug.Log("Cant find path");
                return Result.Failure;
            }

        }
    }
}
