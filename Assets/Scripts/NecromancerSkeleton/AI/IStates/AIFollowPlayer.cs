using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIFollowPlayer : IAIState
{
    public AIFollowPlayer(Transform ai, Transform target)
    {
        s = MinionSettings.settings;
        aiTransform = ai;
        playerTransform = target;


        //Declare an array for all main direction 4 for cardinal and 4 for diagonals
        desiredDirections = new DirDesire[8]
        {
            new DirDesire(Vector2.right),
            new DirDesire(new Vector2(.7f, -.7f)), //Bot Right
            new DirDesire(Vector2.down),
            new DirDesire(new Vector2(-.7f, -.7f)), //Bot Left
            new DirDesire(Vector2.left),
            new DirDesire(new Vector2(-.7f, .7f)), //Top Left
            new DirDesire(Vector2.up),
            new DirDesire(new Vector2(.7f, .7f)) //Top Right
        };
    }
    private Transform playerTransform;
    private Transform aiTransform;
    DirDesire[] desiredDirections;
    private MinionSettings s;

    public void Update(SkeletonInput input)
    {
        Vector2 dir = (playerTransform.position - aiTransform.position).normalized;
        dir = GetDirection(dir);

        input.moveInput = Mathf.InverseLerp(s.minimumFollowDistance,s.slowFollowDistance, Vector2.Distance(playerTransform.position, aiTransform.position)) * dir;
        input.mousePositionInput = playerTransform.position;
    }

    private Vector2 GetDirection(Vector2 targetDir)
    {
        //Go through all directions and set their overall appeal
        for (int i = 0; i < desiredDirections.Length; i++)
        {
            //set Appeal of direction based on how closely it aligns with the target direction
            desiredDirections[i].desire = Vector2.Dot(desiredDirections[i].direction, targetDir);
            desiredDirections[i].desire = CheckCollisionForAppeal(ref desiredDirections[i]);  
        }


        //Sort from best to worst by appeal
        Array.Sort<DirDesire>(desiredDirections, (a, b) => b.desire.CompareTo(a.desire));

        //As long as the direction has positive appeal then allow for some chance for the minion to move that way.
        Vector2 chosenDir = Vector2.zero;
        for (int i = 0; i < desiredDirections.Length && chosenDir == Vector2.zero; i++)
        {
            if(desiredDirections[i].desire >= 0)
            {
                if (UnityEngine.Random.Range(0f, .9f) < desiredDirections[i].desire)
                {
                    chosenDir = desiredDirections[i].direction;
                }
            }
        }

        //Sanity check to ensure a direction is chosen
        if (chosenDir == Vector2.zero)
            chosenDir = desiredDirections[desiredDirections.Length - 1].direction;

        Debug.DrawRay(aiTransform.position, chosenDir * s.pathingRange, Color.green);
        return chosenDir;
    }

    public void Exit()
    {
        
    }

    private float CheckCollisionForAppeal(ref DirDesire dirDes)
    {
        //Check for obstacles
        
        //Cast to hit all object alont the ray path
        RaycastHit2D[] hitObjects = Physics2D.RaycastAll(aiTransform.position, dirDes.direction, s.pathingRange, s.pathingMask);
        //check for more than one collision since the first will always be the minion's own collider
        if (hitObjects.Length > 2)
        {
            RaycastHit2D hit = hitObjects[1];
            // If hit something set appeal to negative
            if (hit)
            {
                Debug.DrawRay(aiTransform.position, dirDes.direction * hit.distance, Color.red);

                //Sanity Check to ensure nothing divides by 0
                if (hit.distance == 0)
                    return -1;
                else
                    return -(s.pathingRange / hit.distance);
            }
        }

        return dirDes.desire;
    }

    private struct DirDesire
    {
        public DirDesire(Vector2 direction)
        {
            this.direction = direction;
            this.desire = 0;
        }

        public Vector2 direction;
        public float desire;
    }
}
