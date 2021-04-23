using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage Skeleton Animator based on variables from other scripts.
/// </summary>
public class SkeletonAnimation : MonoBehaviour
{
    private Animator bodyAnim;
    private SpriteRenderer bodySprite;
    private SkeletonInput input;

    void Start()
    {
        input = GetComponent<SkeletonInput>();
        bodyAnim = transform.GetChild(1).GetChild(0).GetComponent<Animator>();
        bodySprite = transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        //Flip sprite if mouse is to the left of the character so the sprite faces the right way
        bodySprite.flipX = input.mousePositionInput.x < transform.position.x;
        //Set move animator float based on move input
        bodyAnim.SetFloat("Move", input.moveInput.magnitude);
    }
}
