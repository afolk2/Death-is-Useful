using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorTree : MonoBehaviour
{
    public BTFiniteStateMachine stateMachine;

    protected BTNode mRoot;
    private bool startedBehavior;
    public BTNode Root { get { return mRoot; } }
    public Dictionary<string, object> Blackboard { get; set; }
    public virtual void StartTree(BTFiniteStateMachine sm)
    {
        stateMachine = sm;

        Blackboard = new Dictionary<string, object>();
        //initial behavior is stopped
        startedBehavior = false;

        //Example
        //
        ///*mRoot = new BTRepeator(this, new BTSequencer(this, new BTNode[] { new BTRandomWalk(this) }));*/
        //
    }
    public virtual void UpdateTree()
    {
        if (!startedBehavior)
        {
            StartCoroutine(RunBehavior());
            startedBehavior = true;
        }
    }

    public virtual void ExitTree()
    {
        Debug.Log("Exiting Tree to null");
        stateMachine.ClearTree();
    }

    /// <summary>
    /// Called when the behavior tree finishes completely, use this to add a BT the state machine will drop into by default
    /// </summary>
    public virtual void FinishTree(BTNode.Result result)
    {
        Debug.Log("Behavior has finished with : " + result + " This does not have a drop out state. Exiting State");
        ExitTree();
    }

    private IEnumerator RunBehavior()
    {
        BTNode.Result result = Root.Execute();
        while (result == BTNode.Result.Running)
        {
            yield return null;
            result = Root.Execute();
        }
        
        FinishTree(result);
    }

    
}
