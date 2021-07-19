using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine
{
    private IAIState state;
    public void ChangeState(IAIState newState)
    {
        if (newState != null)
        {
            if (state != null && state != newState)
            {
                state.Exit();
            }
            state = newState;
        }
        else
        {
            ExitState();
        }
    }

    public void Update(SkeletonInput input)
    {
        state.Update(input);
    }

    public void ExitState()
    {
        if (state != null)
            state.Exit();

        state = null;
    }
}