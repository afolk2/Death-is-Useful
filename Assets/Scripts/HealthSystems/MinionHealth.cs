using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class MinionHealth : Health
{
    MinionHealthUI healthUI;
    MinionManager mm;
    protected override void SetHealth(int value)
    {
        base.SetHealth(value);
        healthUI.SetUIValue(currentHP, Mathf.InverseLerp(0, maxHP, currentHP));
    }

    protected override void Start()
    {
        mm = MinionManager.manager;
        healthUI = GetComponentInChildren<MinionHealthUI>();
        
        maxHP = mm.minionHP;
        currentHP = maxHP;

        healthUI.SetupUI(currentHP);
        
        dead = false;
    }

    public override void Die()
    {
        base.Die();
        mm.RemoveMinion(GetComponent<MinionController>());
    }

}