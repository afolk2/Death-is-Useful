using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the Hands of the Necromancer/Minion based on their current Kit and their current aim
/// </summary>
public class SkeletonKit : MonoBehaviour
{
    /// <summary>
    /// Holds all of the data for each hand
    /// </summary>
    private struct Hand
    {
        public GameObject handObject;
        public Transform handTransform;
        public SpriteRenderer itemSprite;
        public bool doesItemPoint;
    }
    private Hand mainHand, offHand;

    /// <summary>
    /// Assignable transform, needs to be assigned in order for the script to find the neccessary data points.
    /// </summary>
    [SerializeField] private Transform mainHandTransform, offHandTransform;
    /// <summary>
    /// Adjusts the scale of the stretching the arms of the creature based on the distance of their aim.
    /// </summary>
    [SerializeField] private float aimStretch;
    /// <summary>
    /// The base offset for each of the hands on the creature.
    /// </summary>
    [SerializeField] private Vector2 mainBaseOffset, offBaseOffset;
    /// <summary>
    /// The current offset, defaults to the values above, but otherwise will be set by the current item kit.
    /// </summary>
    private Vector2 mainCurrentOffset, offCurrentOffset;
    /// <summary>
    /// Simple bool to make sure the creature isn't using an action IE. Swinging a sword or casting.
    /// </summary>
    public bool idle = true;

    //Get all of the data and assign them to their respective struct. Reset the current offset to the base.
    private void Start()
    {
        this.mainHand.handTransform = mainHandTransform;
        this.offHand.handTransform = offHandTransform;

        this.mainHand.handObject = mainHandTransform.gameObject;
        this.offHand.handObject = offHandTransform.gameObject;

        this.mainHand.itemSprite = mainHandTransform.GetChild(0).GetComponent<SpriteRenderer>();
        this.offHand.itemSprite = offHandTransform.GetChild(0).GetComponent<SpriteRenderer>();

        mainCurrentOffset = mainBaseOffset;
        offCurrentOffset = offBaseOffset;
    }

    /// <summary>
    /// Called by Skeleton Aim
    /// Adjust the hand position based on the kit, aim, and position relative to aim.
    /// </summary>
    /// <param name="aim"></param>
    /// <param name="leftSideAim"></param>
    public void ItemAim(Vector2 aim, bool leftSideAim)
    {
        //Check if creature is using an action
        if (idle)
        {
            //if not adjust the idle position of each hand. Fliping along the x if the creatures aim is to the left of them. Since the primary hand will want to be on that side instead.

            float mainX = mainCurrentOffset.x * (leftSideAim ? -1f : 1f);

            mainHand.handTransform.localPosition = new Vector2(mainX, mainCurrentOffset.y) + aim.normalized * (Mathf.InverseLerp(0, 5, aim.magnitude) * aimStretch); // < Multiply stretch based on aim magnitude


            //Repeat as above, but subtract the aim and apply aimStretch at half strength. Maybe off hand shouldn't stretch at all.
            float offX = offCurrentOffset.x * (leftSideAim ? -1f : 1f);

            offHand.handTransform.localPosition = new Vector2(offX, offCurrentOffset.y) - aim.normalized * (Mathf.InverseLerp(0, 5, aim.magnitude) * aimStretch / 2);
        }
    }

    /// <summary>
    /// Update the Kit data so sprites and positional data can change.
    /// </summary>
    /// <param name="kit">The new kit being equipped, set to null if unarmed</param>
    public void UpdateActiveKit(Kit kit)
    {
        if(kit == null)
        {
            SetUnarmed();
        }
        else
        {
            ApplyKitToHands(kit);
        }
    }
    /// <summary>
    /// Apply hand data to both be shown, but remove item sprites since unarmed.
    /// </summary>
    private void SetUnarmed()
    {
        this.mainHand.handObject.SetActive(true);
        this.offHand.handObject.SetActive(true);

        this.mainHand.doesItemPoint = true;
        this.offHand.doesItemPoint = false;

        this.mainHand.itemSprite.sprite = null;
        this.offHand.itemSprite.sprite = null;

        mainCurrentOffset = mainBaseOffset;
        offCurrentOffset = offBaseOffset;
    }

    /// <summary>
    /// Set All hand info based on the new kit provided.
    /// </summary>
    /// <param name="newKit"></param>
    private void ApplyKitToHands(Kit newKit)
    {
        this.mainHand.handObject.SetActive(newKit.mainHand.itemSprite != null);
        this.offHand.handObject.SetActive(newKit.offHand.itemSprite != null);

        this.mainHand.doesItemPoint = newKit.mainHand.itemDoesAim;
        this.offHand.doesItemPoint = newKit.offHand.itemDoesAim;

        this.mainHand.itemSprite.sprite = newKit.mainHand.itemSprite;
        this.offHand.itemSprite.sprite = newKit.offHand.itemSprite;

        mainCurrentOffset = newKit.mainHand.itemOffset;
        offCurrentOffset = newKit.offHand.itemOffset;
    }

}
