using Pathfinding;
using UnityEngine;

public class BTAnimateMovement : BTNode
{
    SkeletonAnimation sAnim;
    public BTAnimateMovement(BehaviorTree t) : base(t)
    {
        object o;
        bool found = Tree.Blackboard.TryGetValue("Transform", out o); //Note: replaced o with new object could probably make this more readable
        if (found)
        {
            Transform transform = (Transform)o;
            sAnim = transform.GetComponent<SkeletonAnimation>();
        }
        else
        {
            Debug.LogError("BTAnimateMovement could not find Transform in the Blackboard on " + Tree.transform.name);
        }
    }

    public override Result Execute()
    {
        if (sAnim != null)
        {
            UpdateAnim();
            return Result.Running;
        }
        else
            return Result.Failure;
    }

    private void UpdateAnim()
    {
        sAnim.MoveAnim();

        //anim.SetFloat("Move", Mathf.InverseLerp(aiPath.endReachedDistance, aiPath.slowdownDistance, aiPath.remainingDistance));
        ////Flip sprite if mouse is to the left of the character so the sprite faces the right way
        //if (aiPath.desiredVelocity.x < -0.1)
        //{
        //    bodySprite.flipX = true;
        //}
        //else if (aiPath.desiredVelocity.x > 0.1)
        //{
        //    bodySprite.flipX = false;
        //}

        //bodySprite.sortingOrder = Mathf.RoundToInt(rb.position.y * -1000);
        ////Set move animator float based on move input
    }
}
