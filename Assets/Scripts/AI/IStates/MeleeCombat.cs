using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCombat : IAITask
{
    private EnemyAIBase ai;
    Transform currentTarget;
    AIDestinationSetter destinationSetter;

    //Combat stats
    [SerializeField] private float attackDelay, attackDelayRandomRange;
    [SerializeField] private bool attacking;

    public MeleeCombat(EnemyAIBase ai, Transform initialTarget)
    {
        this.ai = ai;
        currentTarget = initialTarget;
        destinationSetter.target = currentTarget;
    }

    

    protected override void Update()
    {
        if (Vector2.Distance(currentTarget.position, ai.transform.position) > 7f)
        {
            //ChangeState(new EnemyChaseClosestTarget(ai));
        }
    }

    public override void End()
    {
        
    }
}
