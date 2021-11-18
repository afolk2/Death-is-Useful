public class MinionController : AI_BTSM
{
    public enum MinionType { Melee, Ranged, Caster, Tank }
    public int minionIndex;
    protected override void Start()
    {
        aiSystem = new BTFiniteStateMachine(this);
        aiSystem.ChangeTree(new FollowPlayer_BT().GetType());
    }

    // Update is called once per frame
    protected override void Update()
    {
        aiSystem.UpdateTree();
    }
}
