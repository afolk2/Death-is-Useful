using UnityEngine;

public class BTLookForTargets : BTNode
{
    Searcher searcher;
    float searchRange;
    Transform transform;
    public BTLookForTargets(BehaviorTree tree) : base(tree)
    {
        object o;
        bool found = Tree.Blackboard.TryGetValue("SearchRange", out o);
        if (found)
        {
            searchRange = (float)o;
        }

        found = Tree.Blackboard.TryGetValue("Transform", out o);
        if (found)
        {
            transform = (Transform)o;
        }
        else
            Debug.LogError("No Transform on BTLookForTargets");

        found = Tree.Blackboard.TryGetValue("Search", out o);

        if(found)
        {
            searcher = (Searcher)o;
        }    
    }

    public override Result Execute()
    {
        //Adjust this to switch Behavioural Tree based on what was found by Searcher TODO
        Transform foundTarget = null;
        switch (searcher.SearchForType(out foundTarget))
        {
            case Searcher.SearchType.Enemy:
                Debug.Log(foundTarget.name);
                return Result.Success;

            case Searcher.SearchType.Friendly:
                Debug.Log(foundTarget.name);
                return Result.Success;

            case Searcher.SearchType.Loot:
                Debug.Log(foundTarget.name);
                return Result.Success;
        }

        return Result.Running;
    }
}