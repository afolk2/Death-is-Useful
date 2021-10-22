using UnityEngine;
using Pathfinding;
public class MinionFollowPlayer : IAITask
{
    MinionManager minionManager;
    Rigidbody2D rb;

    Transform player;
    SkeletonAim aim;
    AIPath aiPath;
    AIDestinationSetter destinationSetter;
    private Animator bodyAnim;
    private SpriteRenderer bodySprite;

    public MinionFollowPlayer(MinionController minion)
    {
        minionManager = MinionManager.settings;

        aiPath = minion.GetComponent<AIPath>();
        destinationSetter = minion.GetComponent<AIDestinationSetter>();
        rb = minion.GetComponent<Rigidbody2D>();
        aim = minion.GetComponent<SkeletonAim>();

        bodyAnim = minion.transform.GetChild(1).GetChild(0).GetComponent<Animator>();
        bodySprite = minion.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>();

        player = minionManager.GetPlayer();

        //seeker.StartPath(rb.position, player.position, OnPathComplete);

        destinationSetter.enabled = true;
        aiPath.enabled = true;

        destinationSetter.target = player;
    }

    

    protected override void Update()
    {
        //MoveToTarget();
        UpdateAnim();
    }

    private void UpdateAnim()
    {
        //Flip sprite if mouse is to the left of the character so the sprite faces the right way
        if(aiPath.desiredVelocity.x < -0.1)
        {
            bodySprite.flipX = true;
        }
        else if (aiPath.desiredVelocity.x > 0.1)
        {
            bodySprite.flipX = false;
        }    
        bodySprite.sortingOrder = Mathf.RoundToInt(rb.position.y * -1000);
        //Set move animator float based on move input
        bodyAnim.SetFloat("Move", aiPath.velocity.magnitude > 0.9f ? 1 : 0);
        aim.DoAim(player.position);
    }

    public override void End()
    {

    }
}
