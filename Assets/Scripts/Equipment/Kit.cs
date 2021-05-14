using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Just a base idea of how the kit data could be set up. Might change it to an interface down the road.
/// </summary>
public abstract class Kit : MonoBehaviour
{
    /// <summary>
    /// Item data to be passed down to SkeletonKit. Visual Data
    /// </summary>
    [Serializable]
    public struct ItemData
    {
        public Sprite itemSprite;
        public Sprite itemActiveSprite;
        public Vector2 itemOffset;
        public bool itemDoesAim;
        public int spriteLayer;
    }
    [SerializeField]
    /// <summary>
    /// Data for each hand
    /// </summary>
    public ItemData mainHand, offHand;
    /// <summary>
    /// The sprite that displays when the item is loose on the ground
    /// </summary>
    public Sprite dropSprite;
    /// <summary>
    /// Main hand action. Left click
    /// </summary>
    public abstract void MainAction(EquipmentManager manager);
    /// <summary>
    /// 
    /// </summary>
    public abstract void MainRelease(EquipmentManager manager);
    /// <summary>
    /// Off hand action. Right Click
    /// </summary>
    public abstract void SecondaryAction(EquipmentManager manager);
    /// <summary>
    /// 
    /// </summary>
    public abstract void SecondaryRelease(EquipmentManager manager);
}