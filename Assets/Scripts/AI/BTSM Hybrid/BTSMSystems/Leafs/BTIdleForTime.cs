using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTIdleForTime : BTNode
{
    protected float waitTime;
    float timer;
    public BTIdleForTime(BehaviorTree tree) : base(tree)
    {
        object o;
        bool found = Tree.Blackboard.TryGetValue("WaitTime", out o);
        if (found)
        {
            waitTime = (float)o;
        }

        timer = 0;
    }

    public override Result Execute()
    {
        if(timer < waitTime)
        {
            timer += Time.deltaTime;
            return Result.Running;
        }
        else
        {
            timer = 0;
            return Result.Success;
        }
        
    }

}
