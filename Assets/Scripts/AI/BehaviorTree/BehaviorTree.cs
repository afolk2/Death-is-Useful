using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree : MonoBehaviour
{
    private BTNode mRoot;
    private Coroutine behavior;
    private bool startedBehavior;

    public Dictionary<string, object> Blackboard {get; set;}

    public BTNode Root {get { return mRoot; } }

    private void Start()
    {
        Blackboard = new Dictionary<string, object>();
        Blackboard.Add("WorldBounds", new Rect(0, 0, 5, 5));

        //initial behavior is stopped
        startedBehavior = false;

        mRoot = new BTRepeator(this, new BTSequencer(this, new BTNode[] { new BTRandomWalk(this) }));
    }
    private void Update()
    {
        if(!startedBehavior)
        {
            StartCoroutine(RunBehavior());
            startedBehavior = true;
        }
    }

    private IEnumerator RunBehavior()
    {
        BTNode.Result result = Root.Execute();
        while(result == BTNode.Result.Running)
        {
            Debug.Log("Root Result: " + result);
            yield return null;
            result = Root.Execute();
        }
        Debug.Log("Behavior has finished with : " + result);
    }
}
