using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Manages the process of Equiping and unequiping both kits and trinkets. Then pass the new item data along to other classes like SkeletonKit
/// </summary>
public class EquipmentManager : MonoBehaviour
{
    private SkeletonKit skeletonKit;

    private Kit currentKit;
    private Trinket currentTrinket;

    private void Start()
    {
        skeletonKit = GetComponent<SkeletonKit>();
    }

    public void EquipKit(Kit newKit)
    {
        if(currentKit != null)
        {
            //Drop current kit item
        }

        if(newKit != null)
        {
            //Equip new kit

        }
        else
        {
            //Setting kit to null. Unarmed??
        }
    }



    public void EquipTrinket(Trinket newTrinket)
    {
        if (currentTrinket != null)
        {
            //Drop current Trinket
        }

        if (newTrinket != null)
        {
            //Equip new Trinket

        }
    }
}
