using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAim : MonoBehaviour
{
    protected Vector2 aimTarget;
    public Vector2 Aim
    {
        get
        {
            return aimTarget;
        }
    }

    public virtual void SetAimTarget(Vector2 targetPos)
    {
        aimTarget = targetPos - new Vector2(transform.position.x, transform.position.y);
    }
}
