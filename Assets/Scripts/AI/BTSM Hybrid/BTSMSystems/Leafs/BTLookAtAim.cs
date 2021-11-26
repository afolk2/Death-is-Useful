using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLookAtAim : BTNode
{
    NecromancerInput necorInput;
    SkeletonAim aim;
    private SpriteRenderer bodySprite;
    Transform transform;
    public BTLookAtAim(BehaviorTree t) : base(t)
    {
        //GET PLAYER TRANSFORM
        object o;
        bool found = Tree.Blackboard.TryGetValue("NecroInput", out o);

        if (found)
        {
            necorInput = (NecromancerInput)o;
        }
        else
        {
            Debug.LogWarning(Tree.transform.name + "Could not get Necromancer Input");
        }

        found = Tree.Blackboard.TryGetValue("Transform", out o);

        if (found)
        {
            transform = (Transform)o;
            aim = transform.GetComponent<SkeletonAim>();
            bodySprite = transform.GetComponentInChildren<Animator>().GetComponent<SpriteRenderer>();
        }
        else
        {
            Debug.LogWarning(Tree.transform.name + "Transform not found for BTLookAtAim");
        }
    }

    public override Result Execute()
    {
        aim.DoAim(necorInput.mousePositionInput);

        if (transform.position.x > necorInput.mousePositionInput.x)
        {
            bodySprite.flipX = true;
        }
        else
        {
            bodySprite.flipX = false;
        }
        return Result.Running;
    }
}
