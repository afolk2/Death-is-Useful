using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NecroticPower : MonoBehaviour
{
    [SerializeField] private NecroticPowerUI powerBar;
                    
    [SerializeField] private int maxPowerLevel;
    private int powerLevel;

    private void Start()
    {
        powerLevel = maxPowerLevel;
        RefreshUI();
    }

    public bool CheckSummonCost(int cost)
    {
        return cost <= powerLevel;
    }

    public void ChangeMaxPower(int addedPower)
    {
        maxPowerLevel += addedPower;
        powerLevel += addedPower;
        RefreshUI();
    }
    public void ChangePower(int addedPower)
    {
        powerLevel += addedPower;
        RefreshUI();
    }

    private void RefreshUI() => powerBar.SetPower((float)powerLevel / (float)maxPowerLevel);
}