using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyChaseClosestTarget : IAITask
{
    [SerializeField] private float detectionRadius;
    [SerializeField] private LayerMask detectMask;
    [SerializeField] private float distanceToAttack;

    Rigidbody2D rb;
    Transform target;
    AIPath aiPath;
    AIDestinationSetter destinationSetter;
    private SpriteRenderer bodySprite;
    private Animator anim;

    EnemyAIBase c;
    private Transform transform;
    // Start is called before the first frame update
    public EnemyChaseClosestTarget(EnemyAIBase controller)
    {
        c = controller;
        transform = c.transform;
        anim = c.GetComponentInChildren<Animator>();
        aiPath = c.GetComponent<AIPath>();
        destinationSetter = c.GetComponent<AIDestinationSetter>();
        rb = c.GetComponent<Rigidbody2D>();
        bodySprite = anim.GetComponent<SpriteRenderer>();
        destinationSetter.enabled = true;
        aiPath.enabled = true;

        destinationSetter.target = target;
    }

    protected override void Update()
    {
        CheckForTarget();
        if (target != null)
        {
            anim.SetBool("Aggresive", true);
            destinationSetter.target = target;
        }
        else
        {
            destinationSetter.target = null;
            anim.SetBool("Aggresive", false);
        }
        UpdateAnim();
        if (target != null)
            CheckTargetDistance();
    }

    private void CheckForTarget()
    {
        Collider2D[] detected = Physics2D.OverlapCircleAll(transform.position, detectionRadius, detectMask);

        if (detected.Length > 0)
        {
            target = detected[0].transform;
            float dist = Vector3.Distance(transform.position, target.position);


            for (int i = 1; i < detected.Length; i++)
            {
                if (Vector3.Distance(detected[i].transform.position, transform.position) < dist)
                {
                    target = detected[i].transform;
                    dist = Vector3.Distance(transform.position, target.position);
                }
            }
        }
        else
        {
            target = null;
        }
    }

    private void CheckTargetDistance()
    {
        if(Vector2.Distance(target.position, transform.position) < distanceToAttack)
        {
            //.ChangeState(new MeleeCombat(c, target));
        }
    }

    private void UpdateAnim()
    {
        //Flip sprite if mouse is to the left of the character so the sprite faces the right way
        if (aiPath.desiredVelocity.x < -0.1)
        {
            anim.SetBool("Left", true);
        }
        else if (aiPath.desiredVelocity.x > 0.1)
        {
            anim.SetBool("Left", false);
        }
        bodySprite.sortingOrder = Mathf.RoundToInt(rb.position.y * -1000);
    }

    public override void End()
    {

    }
}
