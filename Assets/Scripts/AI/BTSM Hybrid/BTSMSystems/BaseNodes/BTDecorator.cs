using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTDecorator : BTNode
{
    protected BTNode Child { get; set; }
    public BTDecorator(BehaviorTree t, BTNode c) : base(t)
    {
        Child = c;
    }
}
