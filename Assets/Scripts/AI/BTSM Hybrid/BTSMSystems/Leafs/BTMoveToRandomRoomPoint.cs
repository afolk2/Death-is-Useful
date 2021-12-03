using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTMoveToRandomRoomPoint : BTNode
{
    private AIDestinationSetter destSetter;
    private AIPath path;
    protected Transform dest;
    //Just Here to prevent errors while the rest is commented out. Delete Later
    public BTMoveToRandomRoomPoint(BehaviorTree tree) : base(tree)
    {
        dest = new GameObject().transform;
        

        object o;
        bool found = Tree.Blackboard.TryGetValue("Transform", out o);
        if(found)
        {
            Transform transform = (Transform)o;
            dest.name =  transform.name + " Patrol Point";
        }

        FindRandomPatrolPoint();
        

        //GET AIDESTINATIONSETTER AND AIPATH
        found = Tree.Blackboard.TryGetValue("DestinationSetter", out o); //Note: replaced o with new object could probably make this more readable
        if (found)
        {
            destSetter = (AIDestinationSetter)o;
            path = destSetter.transform.GetComponent<AIPath>();
            destSetter.target = dest;
        }
        else
        {
            Debug.LogWarning(Tree.transform.name + " Did not have an AIDestinationSetter attached inside BTMoveToRandomRoomPoint Node. Manually placing one (Fix this tho)");
            destSetter = Tree.gameObject.AddComponent<AIDestinationSetter>();
            path = Tree.gameObject.AddComponent<AIPath>();
        }
    }

    protected void FindRandomPatrolPoint()
    {
        int attempts = 0;
        do
        {
            attempts++;
            dest.position = Random.insideUnitCircle * 11f;
            Debug.DrawLine(dest.position, dest.position + Vector3.right * 3f, Color.green, 1f);
        }
        while (Physics2D.OverlapCircle(dest.position, 1f) || attempts > 50f);
    }

    public override Result Execute()
    {
        if (path.reachedDestination)
        {
            FindRandomPatrolPoint();
            return Result.Success;
        }
        else
        {
            if (path.canMove && path.canSearch && dest != null)
                return Result.Running;
            else
            {
                Debug.Log("Cant Find path or Move");
                return Result.Failure;
            }
        }
    }

}
