using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTRandomWalk : BTNode
{
    //Just Here to prevent errors while the rest is commented out. Delete Later
    public BTRandomWalk(BehaviorTree tree) : base(tree)
    {
    }

    //protected Vector3 NextDestination { get; set; }
    //public float speed;
    //public BTRandomWalk(BehaviorTree t) : base(t)
    //{
    //    NextDestination = Vector3.zero;
    //    FindNextDestination();
    //}

    //protected bool FindNextDestination()
    //{
    //    object o;
    //    bool found = false;

    //    found = Tree.Blackboard.TryGetValue("WorldBounds", out o);

    //    if (found)
    //    {
    //        Rect bounds = (Rect)o;

    //        float x = UnityEngine.Random.value * bounds.width - (bounds.width / 2);
    //        float y = UnityEngine.Random.value * bounds.height - (bounds.height / 2);

    //        NextDestination = new Vector3(x, y, 0);
    //    }


    //    return found;
    //}

    //public override Result Execute()
    //{
    //    //if we've arrived at random position
    //    if (Tree.gameObject.transform.position == NextDestination)
    //    {
    //        if (!FindNextDestination())
    //            return Result.Failure;
    //        else
    //            return Result.Success;
    //    }
    //    else
    //    {
    //        Tree.gameObject.transform.position = Vector3.MoveTowards(Tree.gameObject.transform.position, NextDestination, speed * Time.deltaTime);
    //        return Result.Running;
    //    }     
    //}

}