using Pathfinding;
using UnityEngine;
public class AIAnimation : MonoBehaviour
{
    protected AIPath aiPath;
    protected Animator bodyAnim;
    protected SpriteRenderer bodySprite;

    protected virtual void Start()
    {
        aiPath = transform.GetComponent<AIPath>();
        bodyAnim = transform.GetChild(1).GetChild(0).GetComponent<Animator>();
        bodySprite = transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        bodySprite.sortingOrder = Mathf.RoundToInt(transform.position.y * -1000);
    }
    public virtual void MoveAnim()
    {
        //Flip sprite if mouse is to the left of the character so the sprite faces the right way
        if (aiPath.desiredVelocity.x < -0.1)
        {
            bodySprite.flipX = true;
        }
        else if (aiPath.desiredVelocity.x > 0.1)
        {
            bodySprite.flipX = false;
        }
    }
    public virtual void AimAnim(Vector2 aim)
    {
        bodySprite.flipX = aim.x < transform.position.x;
    }
}
