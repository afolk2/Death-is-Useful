using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class MoveToPoint : IAIState
{
    MinionManager minionManager;
    Rigidbody2D rb;

    Transform player;
    SkeletonAim aim;
    AIPath aiPath;
    AIDestinationSetter destinationSetter;

    float nextWayPointDistance = 3f;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    //Vector2 lastDestination;

    private Animator bodyAnim;
    private SpriteRenderer bodySprite;

    private Transform dest;

    public MoveToPoint(MinionController minion, Vector2 pos)
    {
        dest = new GameObject(null).transform;
        dest.position = pos;

        minionManager = MinionManager.settings;

        aiPath = minion.GetComponent<AIPath>();
        destinationSetter = minion.GetComponent<AIDestinationSetter>();
        rb = minion.GetComponent<Rigidbody2D>();
        aim = minion.GetComponent<SkeletonAim>();

        bodyAnim = minion.transform.GetChild(1).GetChild(0).GetComponent<Animator>();
        bodySprite = minion.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>();

        //seeker.StartPath(rb.position, player.position, OnPathComplete);

        destinationSetter.enabled = true;
        aiPath.enabled = true;

        destinationSetter.target = dest;
    }
    public void Update()
    {
        UpdateAnim();
    }
    private void UpdateAnim()
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
        bodySprite.sortingOrder = Mathf.RoundToInt(rb.position.y * -1000);
        //Set move animator float based on move input
        bodyAnim.SetFloat("Move", aiPath.velocity.magnitude > 0.9f ? 1 : 0);
        aim.DoAim(dest.position);
    }

    public void Exit()
    { 

    }
}
