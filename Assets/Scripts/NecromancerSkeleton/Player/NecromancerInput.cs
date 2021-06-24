using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NecromancerInput : MonoBehaviour
{
    private NecroticPower power;
    [SerializeField] LayerMask interactLayer;
    private void Start()
    {
        power = GetComponent<NecroticPower>();
    }

    public void Interact()
    {
        Collider2D c = Physics2D.OverlapPoint(transform.position, interactLayer);
        SummonableSkeleton summon = c.GetComponentInParent<SummonableSkeleton>();
        if (summon != null)
        {
            if (power.CheckSummonCost(summon.cost))
            {
                summon.Spawn();
                power.ChangePower(-summon.cost);
            }
        }
    }
}