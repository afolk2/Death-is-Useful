using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Finite State Machine to handle transitions between Behavior Trees
public class BTFiniteStateMachine
{
    AI_BTSM aiCore;
    public BTFiniteStateMachine(AI_BTSM core)
    {
        aiCore = core;
    }
    public BehaviorTree activeTree;
    public void ChangeTree(BehaviorTree newTree)
    {
        if(newTree != null)
        {
            if(activeTree != null && newTree != activeTree)
            {
                activeTree.ExitTree();
                aiCore.RemoveTree(activeTree);
            }
            activeTree = newTree;
            activeTree.StartTree(this);
        }
        else
        {
            aiCore.RemoveTree(activeTree);
        }
    }

    public void UpdateTree()
    {
        if (activeTree != null)
            activeTree.UpdateTree();
    }
}
