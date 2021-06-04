using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Basic set up for Trinket, might rework later.
/// </summary>
public abstract class Trinket : MonoBehaviour
{
    public float cooldownLength;
    public abstract void PlayerUseTrinket();
    public bool hasPassive;
    public abstract void ApplyTrinketPassive();
    public bool hasMinionPower;
    internal Sprite dropSprite;

    public abstract void MinionUseTrinket();
}