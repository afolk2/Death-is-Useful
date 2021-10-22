using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BTRepeator : BTDecorator
{
    public BTRepeator(BehaviorTree t, BTNode c) : base(t, c)
    {

    }

    public override Result Execute()
    {
        Debug.Log("Child return: " + Child.Execute());
        return Result.Running;
    }
}
