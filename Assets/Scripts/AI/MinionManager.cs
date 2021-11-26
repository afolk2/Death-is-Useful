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
        playerMoveGuide = FindObjectOfType<PlayerController>().GetComponent<MoveGuide>();

        playerMoveGuide.SetupGuide(.3f);

        for (int i = 0; i < activeMinions.Count; i++)
        {
            activeMinions[i].minionIndex = i;
            playerMoveGuide.AddSubPoint();
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
        powerLevel = maxPowerLevel;
        RefreshUI();
    }

    public Transform GetPlayerCoreMoveGuide()
    {
        return playerMoveGuide.coreGuide;
    }

    public Transform GetPlayerSubMoveGuide(int index)
    {
        if (index < playerMoveGuide.coreGuide.childCount)
            return playerMoveGuide.GetSubGuide(index);
        else
            return null;
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
            activeMinions[i].aiSystem.ChangeTree<MoveToCommandPoint_BT>();
        }
    }

    public void FollowPlayer()
    {
        movePoint.GetComponentInChildren<ParticleSystem>().Stop();
        //LeanTween.scale(movePoint.gameObject, Vector3.zero, 4f);
        Destroy(movePoint.gameObject);

        for (int i = 0; i < activeMinions.Count; i++)
        {
            activeMinions[i].aiSystem.ChangeTree<FollowPlayer_BT>();
        }
    }

    public void AddMinion(MinionController newMinion)
    {
        newMinion.minionIndex = activeMinions.Count;
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
