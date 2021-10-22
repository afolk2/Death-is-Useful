using Pathfinding;
using UnityEngine;

public class IdlePatrol : IAITask
{
    private EnemyAIBase ai;
    Transform idleTransform;
    Vector2 startPos;
    AIDestinationSetter destinationSetter;
    public IdlePatrol(EnemyAIBase ai)
    {
        this.ai = ai;
        idleTransform = GameObject.Instantiate(new GameObject(null), ai.transform.position, Quaternion.identity).transform;
        startPos = ai.transform.position;
        destinationSetter.target = idleTransform;
    }

    protected override void Update()
    {
        idleTransform.position = (Random.insideUnitCircle * 2f) + startPos;
    }

    public override void End()
    {

    }
}
