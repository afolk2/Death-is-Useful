using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sequencer but requires Success to move onto next child node
/// </summary>
public class BTStepSequencer : BTSequencer
{
    public BTStepSequencer(BehaviorTree t, BTNode[] children) : base(t, children)
    {
    }

    public override Result Execute()
    {
        if (currentNode < Children.Count)
        {
            Result result = Children[currentNode].Execute();

            if (result == Result.Success)
            {
                currentNode++;
                if (currentNode < Children.Count)
                    return Result.Running;
                else
                {
                    currentNode = 0;
                    return Result.Success;
                }
            }
            else
                return Result.Failure;
        }

        return Result.Success;
    }
}
