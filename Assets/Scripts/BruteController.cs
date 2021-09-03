using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BruteController : MonoBehaviour
{
    MinionManager minionManager;
    Rigidbody2D rb;

    Transform player;
    //SkeletonAim aim;
    AIPath aiPath;
    AIDestinationSetter destinationSetter;

    float nextWayPointDistance = 3f;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    private SpriteRenderer bodySprite;



    // Start is called before the first frame update
    void Start()
    {
        minionManager = MinionManager.settings;

        aiPath = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        rb = GetComponent<Rigidbody2D>();
        //aim = GetComponent<SkeletonAim>();

        //bodyAnim = transform.GetChild(1).GetChild(0).GetComponent<Animator>();

        bodySprite = transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>();

        player = minionManager.GetPlayer();

        //seeker.StartPath(rb.position, player.position, OnPathComplete);

        destinationSetter.enabled = true;
        aiPath.enabled = true;

        destinationSetter.target = player;
    }

    public void Update()
    {
        //MoveToTarget();
        UpdateAnim();
    }
    private void UpdateAnim()
    {
        //Flip sprite if mouse is to the left of the character so the sprite faces the right way
        if (aiPath.desiredVelocity.x < -0.1)
        {
            bodySprite.flipX = !true;
        }
        else if (aiPath.desiredVelocity.x > 0.1)
        {
            bodySprite.flipX = !false;
        }
        bodySprite.sortingOrder = Mathf.RoundToInt(rb.position.y * -1000);
        //Set move animator float based on move input
        //bodyAnim.SetFloat("Move", aiPath.velocity.magnitude > 0.9f ? 1 : 0);
        //aim.DoAim(player.position);
    }
}
