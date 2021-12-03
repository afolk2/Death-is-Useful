using UnityEngine;
public class SkeletonAnimation : AIAnimation
{
    private void Update()
    {
        bodySprite.sortingOrder = Mathf.RoundToInt(transform.position.y * -1000);
    }
    public override void MoveAnim()
    {
        bodyAnim.SetFloat("Move", Mathf.InverseLerp(aiPath.endReachedDistance, aiPath.slowdownDistance, aiPath.remainingDistance));
    }
}
