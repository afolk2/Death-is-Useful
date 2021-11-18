using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MinionManager : MonoBehaviour
{
    #region Singleton
    public static MinionManager manager;
    private void Awake()
    {
        if (manager == null)
        {
            manager = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        #endregion
        //necromancerPosGuide = FindObjectOfType<PlayerController>().GetComponent<MoveGuide>().movementGuide;
        activeMinions = FindObjectsOfType<MinionController>().ToList();

        for (int i = 0; i < activeMinions.Count; i++)
        {
            activeMinions[i].minionIndex = i;
        }

    }

    

    [SerializeField] private NecroticPowerUI powerBar;
    [SerializeField] private int maxPowerLevel;
    private int powerLevel;
    private List<MinionController> activeMinions;
    public float minimumFollowDistance, slowFollowDistance;

    [SerializeField] private GameObject movePrefab;

    private Transform movePoint;
    private MoveGuide playerMoveGuide;

    public Transform GetTarget()
    {
        return movePoint;
    }

    private void Start()
    {
        playerMoveGuide = FindObjectOfType<PlayerController>().GetComponent<MoveGuide>();
        powerLevel = maxPowerLevel;
        RefreshUI();
    }

    public Transform GetPlayerMoveGuide()
    {
        return playerMoveGuide.coreGuide;
    }

    //TODO implement different groups of minions
    public void MoveHere(int commandIndex, Vector2 pos)
    {
        if (movePoint == null)
        {
            movePoint = Instantiate(movePrefab).transform;
        }
        movePoint.position = pos;
        for (int i = 0; i < activeMinions.Count; i++)
        {
            activeMinions[i].aiSystem.ChangeTree(new MoveToCommandPoint_BT().GetType());
        }
    }

    public void FollowPlayer()
    {
        movePoint.GetComponentInChildren<ParticleSystem>().Stop();
        //LeanTween.scale(movePoint.gameObject, Vector3.zero, 4f);
        Destroy(movePoint.gameObject);

        for (int i = 0; i < activeMinions.Count; i++)
        {
            activeMinions[i].aiSystem.ChangeTree(new FollowPlayer_BT().GetType());
        }
    }

    public void AddMinion(MinionController newMinion)
    {
        activeMinions.Add(newMinion);
        playerMoveGuide.AddSubPoint();
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
