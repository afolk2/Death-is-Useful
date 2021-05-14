﻿using System;
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
    /// Manages the sprite sorting layers of the hands and their item sprites
    /// </summary>
    private KitSprite kitSpriteManager;



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

        kitSpriteManager = GetComponent<KitSprite>();

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
            // If an aiming item (like a staff or bow) then rotate hand to point at aim
            if (mainHand.doesItemPoint)
            {
                float angle = -90 + Mathf.Atan2(aim.normalized.y, aim.normalized.x) * Mathf.Rad2Deg;
                mainHand.handTransform.localEulerAngles = new Vector3(0, 0, angle);
            }
            else
                mainHand.handTransform.localEulerAngles = Vector3.zero;



            //Repeat as above, but subtract the aim and apply aimStretch at half strength. Maybe off hand shouldn't stretch at all.
            float offX = offCurrentOffset.x * (leftSideAim ? -1f : 1f);
            offHand.handTransform.localPosition = new Vector2(offX, offCurrentOffset.y) - aim.normalized * (Mathf.InverseLerp(0, 5, aim.magnitude) * aimStretch / 2);
            // If an aiming item (like a staff or bow) then rotate hand to point at aim
            if (offHand.doesItemPoint)
            {
                float angle = -90 + Mathf.Atan2(aim.normalized.y, aim.normalized.x) * Mathf.Rad2Deg;
                offHand.handTransform.localEulerAngles = new Vector3(0, 0, angle);
            }
            else
                offHand.handTransform.localEulerAngles = Vector3.zero;
        }
    }


    public void StartMeleeAttack(Vector2 aim, float arcValue, float reach, float anticipationTime, float swingTime, float recoveryTime, bool swingSwap, bool useMain)
    {
        //Set Angle at the top (this locks the aim for the attack)
        float angle = -90 + Mathf.Atan2(aim.normalized.y, aim.normalized.x) * Mathf.Rad2Deg;
        //Get the starting and end angles of the swing. Swap the values if making a back swing with the weapon
        float startAng = swingSwap ? angle - (arcValue / 2) : angle + (arcValue / 2);
        float endAng = swingSwap ? angle + (arcValue / 2) : angle - (arcValue / 2);


        StartCoroutine(SwingMelee(startAng, endAng, reach, anticipationTime, swingTime, recoveryTime, useMain));
    }

    private IEnumerator SwingMelee(float start, float end, float reach, float anticipationTime, float swingTime, float recoveryTime, bool useMain)
    {
        idle = false;
        if (useMain)
        {
            Vector3 startPos = mainHand.handTransform.localPosition;
            float startRot = mainHand.handTransform.localEulerAngles.z;

            LeanTween.moveLocal(mainHand.handObject, MathHelpers.DegreeToVector2(start), anticipationTime);
            LeanTween.rotateZ(mainHand.handObject, start, anticipationTime);
            yield return new WaitForSeconds(anticipationTime);

            //Activate weapon trail

            LeanTween.moveLocal(mainHand.handObject, MathHelpers.DegreeToVector2(end), swingTime);
            LeanTween.rotateZ(mainHand.handObject, end, swingTime);

            yield return new WaitForSeconds(swingTime);
            // end swing

            LeanTween.moveLocal(mainHand.handObject, startPos, recoveryTime);
            LeanTween.rotateZ(mainHand.handObject, startRot, recoveryTime);

            yield return new WaitForSeconds(recoveryTime);
        }
        else
        {
            Vector3 startPos = offHand.handTransform.localPosition;
            float startRot = offHand.handTransform.localEulerAngles.z;

            offHand.handTransform.localPosition = MathHelpers.DegreeToVector2(start);
            offHand.handTransform.localEulerAngles = new Vector3(0, 0, start);

            //Activate weapon trail

            LeanTween.moveLocal(offHand.handObject, MathHelpers.DegreeToVector2(end), swingTime);
            LeanTween.rotateZ(offHand.handObject, end, swingTime);

            yield return new WaitForSeconds(swingTime);
            // end swing

            LeanTween.moveLocal(offHand.handObject, startPos, recoveryTime);
            LeanTween.rotateZ(offHand.handObject, startRot, recoveryTime);

            yield return new WaitForSeconds(recoveryTime);
        }
        idle = true;
    }

    /// <summary>
    /// Update the Kit data so sprites and positional data can change.
    /// </summary>
    /// <param name="kit">The new kit being equipped, set to null if unarmed</param>
    public void UpdateActiveKit(Kit kit)
    {
        if (kit == null)
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
        mainHand.handObject.SetActive(true);
        offHand.handObject.SetActive(true);

        mainHand.doesItemPoint = true;
        offHand.doesItemPoint = false;

        mainHand.itemSprite.sprite = null;
        offHand.itemSprite.sprite = null;

        mainCurrentOffset = mainBaseOffset;
        offCurrentOffset = offBaseOffset;

        kitSpriteManager.SetKitLayers(0, 0);
    }

    /// <summary>
    /// Set All hand info based on the new kit provided.
    /// </summary>
    /// <param name="newKit"></param>
    private void ApplyKitToHands(Kit newKit)
    {
        mainHand.handObject.SetActive(newKit.mainHand.itemSprite != null);
        offHand.handObject.SetActive(newKit.offHand.itemSprite != null);

        mainHand.doesItemPoint = newKit.mainHand.itemDoesAim;
        offHand.doesItemPoint = newKit.offHand.itemDoesAim;

        mainHand.itemSprite.sprite = newKit.mainHand.itemSprite;
        offHand.itemSprite.sprite = newKit.offHand.itemSprite;

        mainCurrentOffset = newKit.mainHand.itemOffset;
        offCurrentOffset = newKit.offHand.itemOffset;

        kitSpriteManager.SetKitLayers(newKit.mainHand.spriteLayer, newKit.offHand.spriteLayer);
    }

}
