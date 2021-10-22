public class MinionController : AI_BTSM
{
    protected override void Start()
    {
        aiSystem = new BTFiniteStateMachine(this);
        aiSystem.ChangeTree(gameObject.AddComponent<FollowPlayer_BT>());
    }

    // Update is called once per frame
    protected override void Update()
    {
        aiSystem.UpdateTree();
    }
}
