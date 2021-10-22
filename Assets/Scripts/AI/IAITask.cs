using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAITask
{
    public string taskID;
    public enum UpdateState { delta, fixedDelta, custom };
    public UpdateState updateState;
    public float updateTime;
    protected abstract void Update();
    protected IEnumerator Run()
    {
        switch (updateState)
        {
            case UpdateState.delta:
                yield return new WaitForEndOfFrame();
                Update();
                break;
            case UpdateState.fixedDelta:
                yield return new WaitForFixedUpdate();
                Update();
                break;
            case UpdateState.custom:
                if(updateTime > 0f)
                {
                    yield return new WaitForSeconds(updateTime);
                    Update();
                } 
                break;
        }
    }


    public abstract void End();
}
