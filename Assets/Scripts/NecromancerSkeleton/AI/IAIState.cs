using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAIState
{
    public void Update(SkeletonInput input);
    public void Exit();
}
