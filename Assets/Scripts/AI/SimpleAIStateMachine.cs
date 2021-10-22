/// <summary>
/// This AI only has a single update track, acting on only one task at a time
/// </summary>
public class SimpleAIStateMachine : AIStateMachine
{
    private IAITask state;
    public void ChangeUpdateTime(float newSpeed)
    {
        state.updateState = IAITask.UpdateState.custom;
        state.updateTime = newSpeed;
    }
    public void ChangeToDelta()
    {
        state.updateState = IAITask.UpdateState.delta;
    }
    public void ChangeToFixed()
    {
        state.updateState = IAITask.UpdateState.fixedDelta;
    }

    public void ChangeState(IAITask newState, float updateSpeed)
    {
        state = newState;
        state.updateState = IAITask.UpdateState.custom;
        state.updateTime = updateSpeed;

    }
    public void ChangeState(IAITask newState)
    {
        state = newState;
        state.updateState = IAITask.UpdateState.delta;

    }
    public IAITask Task
    {
        set
        {
            ChangeState(value);
        }
        get
        {
            return state;
        }
    }
}
