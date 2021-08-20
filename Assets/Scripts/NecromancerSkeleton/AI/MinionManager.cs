using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MinionManager : MonoBehaviour
{
    #region Singleton
    public static MinionManager settings;
    private void Awake()
    {
        if (settings == null)
        {
            settings = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        #endregion
        necromancer = FindObjectOfType<NecromancerInput>().transform;
        activeMinions = FindObjectsOfType<MinionController>().ToList();
    }
   

    private Transform necromancer;
    public enum MinionType { Melee, Ranged, Caster, Tank }

    [SerializeField] private NecroticPowerUI powerBar;
    [SerializeField] private int maxPowerLevel;
    private int powerLevel;
    private List<MinionController> activeMinions;
    public float minimumFollowDistance, slowFollowDistance;

    private void Start()
    {
        powerLevel = maxPowerLevel;
        RefreshUI();
    }

    public Transform GetPlayer()
    {
        return necromancer;
    }

    //TODO implement different groups of minions
    public void MoveHere(int commandIndex, Vector2 pos)
    {
        for (int i = 0; i < activeMinions.Count; i++)
        {
            activeMinions[i].sm.ChangeState(new MoveToPoint(activeMinions[i] ,pos));
        }
    }

    public void FollowPlayer()
    {
        for (int i = 0; i < activeMinions.Count; i++)
        {
            activeMinions[i].sm.ChangeState(new AIFollowPlayer(activeMinions[i]));
        }
    }

    public void AddMinion(MinionController newMinion)
    {
        activeMinions.Add(newMinion);
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