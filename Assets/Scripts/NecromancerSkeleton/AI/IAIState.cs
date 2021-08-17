using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAIState
{
    public void Update();
    public void Exit();
}
