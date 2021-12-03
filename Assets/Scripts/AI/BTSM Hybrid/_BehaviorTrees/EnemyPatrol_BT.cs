using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol_BT : BehaviorTree, IDamage, IHeal, IKnockback
{
    public override void StartTree(BTFiniteStateMachine sm)
    {
        base.StartTree(sm);
        Blackboard.Add("DestinationSetter", GetComponent<Pathfinding.AIDestinationSetter>());
        Blackboard.Add("Transform", transform);
        Blackboard.Add("WaitTime", 5f);
        Blackboard.Add("SearhLayer", GetComponent<Searcher>());
        mRoot = new BTRepeator
            (this, new BTParallel
                (this, new BTNode[]
                {
                    new BTStepSequencer
                       (this, new BTNode[]
                            {
                                new BTParallel
                                (this, new BTNode[]
                                      { new BTMoveToRandomRoomPoint(this) , new BTAnimateMovement(this) }
                                ),
                                new BTIdleForTime(this)
                            }
                       ),
                    new BTLookForTargets(this)
                })
            );
    }

    public override void ExitTree()
    {
        Debug.Log("No longer following player");
    }

    public void Damage(int damage)
    {

    }

    public void Heal(int heal)
    {

    }

    public void Knockback(float force)
    {

    }
}
