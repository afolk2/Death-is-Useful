using UnityEngine;

public class Searcher : MonoBehaviour
{
    [SerializeField] private float searchRange;
    public LayerMask opponentMask, friendlyMask, lootMask;

    public enum SearchType { Enemy, Friendly, Loot, None};

    public SearchType SearchForType(out Transform foundTarget)
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, searchRange);
        foundTarget = collider.transform;
        GameObject target = collider.gameObject;

        if (opponentMask == (opponentMask | (1 << gameObject.layer)))
        {
            return SearchType.Enemy;
        }
        else if (friendlyMask == (opponentMask | (1 << gameObject.layer)))
        {
            return SearchType.Friendly;
        }
        else if (lootMask == (opponentMask | (1 << gameObject.layer)))
        {
            return SearchType.Loot;
        }
        else
            return SearchType.None;

    }
}