using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IAITask;

public abstract class EnemyAIBase : MonoBehaviour
{
    protected SimpleAIStateMachine sm;
    
    protected virtual void Start()
    {
        sm = new SimpleAIStateMachine();
        sm.ChangeState(new IdlePatrol(this), 4f);
        StartCoroutine(AIUpdate());
    }

    protected virtual IEnumerator AIUpdate()
    {
        switch (sm.Task.updateState)
        {
            case UpdateState.delta:
                yield return new WaitForEndOfFrame();
                break;
            case UpdateState.fixedDelta:
                yield return new WaitForFixedUpdate();
                break;
            case UpdateState.custom:
                if (sm.Task.updateTime > 0f)
                    yield return new WaitForSeconds(sm.Task.updateTime);
                break;
        }

        //sm.Task.Update();
    }

    
}
