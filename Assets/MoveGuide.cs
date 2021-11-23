using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGuide : MonoBehaviour
{
    /// <summary>
    /// The maxe value the guide will move away from the player
    /// </summary>
    [SerializeField] private float guideOffsetMax;
    /// <summary>
    /// What the Guide will be blocked by. Prevents the guide moving past walls so follower wont break;
    /// </summary>
    [SerializeField] private LayerMask guideMask;

    /// <summary>
    /// The Core Guide for movement
    /// </summary>
    public Transform coreGuide;

    private float characterRadius;

    [SerializeField] private float subGuideSpacing = 2f;
    [SerializeField] private float subGuideRot = 15f;
    /// <summary>
    /// Sub Transforms of core guide, used to set wanted destination for followers.
    /// </summary>
    private List<Transform> subGuide;

    // Start is called before the first frame update
    public void SetupGuide(float radius)
    {
        this.characterRadius = radius;

        coreGuide = new GameObject().transform;
        coreGuide.parent = transform;
        coreGuide.localPosition = Vector3.zero;

        coreGuide.name = "MoveGuide";

        subGuide = new List<Transform>();
    }

    public void UpdateGuides(Vector2 moveInput, Vector2 mousePos)
    {
        SetCoreGuidePos(moveInput);
        SetCoreAngle(mousePos);
        SetSubGuide();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="moveInput"></param>
    private void SetCoreGuidePos(Vector2 moveInput)
    {
        //Set Position of Core Guide
        if (moveInput != Vector2.zero)
        {
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, characterRadius, moveInput, guideOffsetMax, guideMask);
            if (hit.collider == null)
            {
                coreGuide.localPosition = moveInput * guideOffsetMax;
            }
            else
            {
                coreGuide.position = hit.point;
            }
        }
        else
            coreGuide.localPosition = Vector2.zero;
    }

    /// <summary>
    /// Set Angle of Core Guide
    /// </summary>
    /// <param name="mousePos">Current Mouse Position</param>
    private void SetCoreAngle(Vector2 mousePos)
    {
        Vector2 direction = ((Vector3)mousePos - coreGuide.position).normalized;
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90f;
        coreGuide.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    /// <summary>
    /// Refresh Sub Guide positions
    /// </summary>
    private void SetSubGuide()
    {
        //Place points in formation starting from the Vector.down of the core guide
        if (subGuide.Count > 0)
        {
            if (subGuide.Count % 2 == 0)
            {
                /// Even Distribution seems mostly work, however it doesnt spawn them in a sensible order, nor does it keep symertical. 
                /// Likely will need to recommit to placing 2 dots at the same time
                /// remember to double arc space for double points for determinding circle num
                //Even
                for (int i = 1; i < subGuide.Count / 2 + 1; i++)
                {
                    int circleNum = (int)((subGuideSpacing * (i * 2)) / (2 * Mathf.PI * subGuideSpacing)) + 1;
                    float crowdRadius = subGuideSpacing * circleNum;
                    float theta = (subGuideSpacing / crowdRadius);

                    float x = crowdRadius * Mathf.Cos(theta * (i - 1) + (subGuideRot * Mathf.Deg2Rad));
                    float y = crowdRadius * Mathf.Sin(theta * (i - 1) + (subGuideRot * Mathf.Deg2Rad));

                    AttemptSubGuidePlacement(i - 1, new Vector2(x, circleNum > 1 ? y : -y));
                    AttemptSubGuidePlacement(i - 1 + (subGuide.Count / 2), new Vector2(-x, circleNum > 1 ? y : -y));
                }
            }
            else
            {
                AttemptSubGuidePlacement(0, Vector2.down * subGuideSpacing);

                /// Odd distribution not tested. Likely not working at all. 
                //Even
                for (int i = 1; i < subGuide.Count / 2 + 1; i++)
                {
                    int circleNum = (int)((subGuideSpacing * (i * 2)) / (2 * Mathf.PI * subGuideSpacing)) + 1;
                    float crowdRadius = subGuideSpacing * circleNum;
                    float theta = (subGuideSpacing / crowdRadius);

                    float x = crowdRadius * Mathf.Cos(theta * (i - 1) + (subGuideRot * Mathf.Deg2Rad));
                    float y = crowdRadius * Mathf.Sin(theta * (i - 1) + (subGuideRot * Mathf.Deg2Rad));

                    AttemptSubGuidePlacement(i , new Vector2(x, circleNum > 1 ? y : -y));
                    AttemptSubGuidePlacement(i + (subGuide.Count / 2), new Vector2(-x, circleNum > 1 ? y : -y));
                }

            }
        }
    }

    private void AttemptSubGuidePlacement(int subIndex, Vector3 pos)
    {
        Vector2 posDir = (pos - coreGuide.position).normalized;
        RaycastHit2D hit = Physics2D.CircleCast(coreGuide.position, characterRadius, posDir, pos.magnitude, guideMask);
        if (hit.collider == null)
        {
            subGuide[subIndex].localPosition = pos;
        }
        else
        {
            subGuide[subIndex].position = hit.point;
        }
    }

    public void AddSubPoint()
    {
        Transform sub = new GameObject().transform;
        sub.parent = coreGuide;
        sub.localPosition = Vector3.zero;

        sub.name = "Sub :" + subGuide.Count.ToString();
        subGuide.Add(sub);
    }

    public void RemoveSubPoint()
    {
        subGuide.RemoveAt(subGuide.Count - 1);
    }
    private void OnDrawGizmos()
    {
        if (coreGuide != null)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawLine(transform.position, coreGuide.position);
            Gizmos.DrawWireSphere(coreGuide.position, characterRadius);

            if (subGuide.Count > 0)
            {
                Gizmos.color = Color.blue;
                for (int i = 0; i < subGuide.Count; i++)
                {
                    Gizmos.DrawLine(coreGuide.position, subGuide[i].position);
                    Gizmos.DrawWireSphere(subGuide[i].position, characterRadius);
                }
            }

        }
    }
}
