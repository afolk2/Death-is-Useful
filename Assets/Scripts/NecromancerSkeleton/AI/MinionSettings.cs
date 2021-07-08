using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSettings : MonoBehaviour
{
    #region Singleton
    public static MinionSettings settings;
    private void Awake()
    {
        if (settings == null)
        {
            settings = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    public float minimumFollowDistance, slowFollowDistance;
    public float pathingRange;
    public LayerMask pathingMask;


}
