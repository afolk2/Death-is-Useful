using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class MinionController : MonoBehaviour
{
    public AIStateMachine sm;

    void Start()
    {
        sm = new AIStateMachine();
        sm.ChangeState(new AIFollowPlayer(this));
    }

    // Update is called once per frame
    void Update()
    {
        sm.Update();
    }
}
