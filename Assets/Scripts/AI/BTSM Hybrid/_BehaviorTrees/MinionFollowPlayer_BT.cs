using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MinionFollowPlayer_BT : BehaviorTree, IDamage, IHeal, IKnockback
{
    public override void StartTree(BTFiniteStateMachine sm)
    {
        base.StartTree(sm);
        Transform followPoint = MinionManager.manager.GetPlayerSubMoveGuide(transform.GetComponent<MinionController>().minionIndex);
        Blackboard.Add("FollowTransform", followPoint != null ? followPoint : MinionManager.manager.GetPlayerCoreMoveGuide());
        Blackboard.Add("DestinationSetter", GetComponent<Pathfinding.AIDestinationSetter>());
        Blackboard.Add("Transform", transform);
        Blackboard.Add("NecroInput", FindObjectOfType<NecromancerInput>());

        mRoot = new BTRepeator
            (this, new BTStepSequencer
                   (this, new BTNode[]
                        {   
                            new BTParallel
                            (this, new BTNode[]
                                  { new BTMoveToPlayer(this), new BTAnimateMovement(this) }
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
