using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Finite State Machine to handle transitions between Behavior Trees
public class BTFiniteStateMachine
{
    AI_BTSM aiCore;

    Type previousBT;

    public BTFiniteStateMachine(AI_BTSM core)
    {
        aiCore = core;
    }

    public void ChangeTree<T>()
    {
        SwitchTree(typeof(T));
    }

    public BehaviorTree activeTree;

    private void SwitchTree(Type tree)
    {
        if (tree != null)
        {
            //Check to see if currently using another behavior tree
            if (activeTree != null)
            {
                if (tree == activeTree.GetType())
                {
                    //Catch in case already using this behavior tree
                    return;
                }

                activeTree.ExitTree();
                aiCore.RemoveTree(activeTree);
            }

            BehaviorTree newTree = (BehaviorTree)aiCore.gameObject.AddComponent(tree);

            activeTree = newTree;
            activeTree.StartTree(this);
        }
        else
        {
            aiCore.RemoveTree(activeTree);
        }
    }

    public void ClearTree()
    {
        aiCore.RemoveTree(activeTree);
    }

    public void UpdateTree()
    {
        if (activeTree != null)
            activeTree.UpdateTree();
    }
}
