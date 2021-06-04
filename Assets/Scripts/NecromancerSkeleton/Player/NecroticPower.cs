using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NecroticPower : MonoBehaviour
{
    [SerializeField] private NecroticPowerUI powerBar;

    private int powerLevel;
    private int maxPowerLevel;

    private void Start()
    {
        MaxPowerLevel = 10;
        powerLevel = 10;
    }


    public int MaxPowerLevel
    {
        get { return maxPowerLevel; }

        set
        {
            int difference = value - maxPowerLevel;
            maxPowerLevel = value;
            CurrentPowerLevel += difference;
        }
    }


    public int CurrentPowerLevel
    {
        get { return powerLevel; }
        set
        {
            powerLevel = Mathf.Clamp(value, 0, MaxPowerLevel);
            powerBar.SetPower(powerLevel / maxPowerLevel);
        }
    }


    public void TrySummoning(SummonableSkeleton summon)
    {
        if(summon.cost < powerLevel)
        {

        }
    }

}