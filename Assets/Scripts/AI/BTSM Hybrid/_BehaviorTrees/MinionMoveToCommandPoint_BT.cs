using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MinionMoveToCommandPoint_BT : BehaviorTree, IDamage, IHeal, IKnockback
{
    Transform target;
    public override void StartTree(BTFiniteStateMachine sm)
    {
        base.StartTree(sm);
        Blackboard.Add("DestinationSetter", GetComponent<Pathfinding.AIDestinationSetter>());
        Blackboard.Add("Transform", transform);
        Blackboard.Add("DestinationSetter", GetComponent<Pathfinding.AIDestinationSetter>());
        Blackboard.Add("Transform", transform);
        Blackboard.Add("NecroInput", FindObjectOfType<NecromancerInput>());

        target = MinionManager.manager.GetTarget();

        mRoot = new BTRepeator
            (this, new BTStepSequencer
                   (this, new BTNode[]
                        {
                            new BTParallel
                            (this, new BTNode[]
                                  { new BTMoveToTarget(this, target), new BTAnimateMovement(this) }
                            ),
                            new BTParallel
                            (this, new BTNode[]
                                  { new BTLookAtAim(this), new BTAnimateMovement(this) }
                            )
                        }
                   )
            );
    }
    public override void ExitTree()
    {
        Debug.Log("No longer moving to command point");
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
