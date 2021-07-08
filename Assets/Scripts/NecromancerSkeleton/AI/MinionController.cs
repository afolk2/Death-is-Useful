using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionController : MonoBehaviour
{
    NecromancerInput player;
    private SkeletonInput skeletonInput;
    private AIStateMachine sm;
    void Start()
    {
        sm = new AIStateMachine();

        player = FindObjectOfType<NecromancerInput>();
        skeletonInput = GetComponent<SkeletonInput>();
        sm.ChangeState(new AIFollowPlayer(transform, player.transform));
    }

    // Update is called once per frame
    void Update()
    {
        sm.Update(skeletonInput);
    }
}
