using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTParallel : BTComposite
{
    public BTParallel(BehaviorTree t, BTNode[] children) : base(t, children)
    {

    }

    public override Result Execute()
    {
        Result r = Result.Running;

        for (int i = 0; i < Children.Count; i++)
        {
            r = Children[i].Execute();
            if (r != Result.Running)
                break;
        }
        return r;
    }
}
