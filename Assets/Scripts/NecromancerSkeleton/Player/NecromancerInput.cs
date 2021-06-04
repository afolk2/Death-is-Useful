using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NecromancerInput : MonoBehaviour
{
    private NecroticPower power;

    private void Start()
    {
        power = GetComponent<NecroticPower>();
    }

    public void Interact()
    {
        SummonableSkeleton summon = Physics2D.OverlapPoint(transform.position).GetComponent<SummonableSkeleton>();
        if(summon.cost < power.CurrentPowerLevel)
        {
            summon.Spawn();
            power.CurrentPowerLevel -= summon.cost;
        }
    }
}