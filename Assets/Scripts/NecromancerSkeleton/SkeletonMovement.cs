using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles Movement on the Necromancer/Minion
public class SkeletonMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    public void Move(Vector2 move)
    {
        transform.Translate(move * Time.deltaTime * 5);
    }
}
