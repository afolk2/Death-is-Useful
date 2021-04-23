using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Just a base idea of how the kit data could be set up. Might change it to an interface down the road.
/// </summary>
public abstract class Kit
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
    }

    /// <summary>
    /// Data for each hand
    /// </summary>
    public ItemData mainHand, offHand;
    /// <summary>
    /// Main hand action. Left click
    /// </summary>
    public abstract void MainAction();
    /// <summary>
    /// Off hand action. Right Click
    /// </summary>
    public abstract void SecondaryAction();
}