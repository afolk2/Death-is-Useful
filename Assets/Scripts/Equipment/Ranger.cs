using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger : Kit
{
    [Header("-- Bow Attack Stats --")]
    [SerializeField] private int bowDamage;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float reloadTime;

    [SerializeField] private GameObject boltPrefab;

    [SerializeField] private Sprite[] crossbowSprites;
    private bool reloading;
    private bool loaded;
    public int ammo;

    [Header("-- Dagger Attack Stats --")]
    [SerializeField] private int damage;
    [SerializeField] private float reach;
    [SerializeField] private float swingTime;

    private bool backSlash;

    [SerializeField] private LayerMask hitMask;

    public override void MainAction(EquipmentManager manager)
    {
        if (manager.skeletonKit.idle)
        {
            if (loaded)
            {
                //FIRE
                loaded = false;
                StartCoroutine(FireBolt(manager.skeletonKit));
                manager.skeletonKit.SetMainSprite(crossbowSprites[0]);
            }
            else
            {
                reloading = true;
                StartCoroutine(Reload(manager.skeletonKit));
            }

        }
    }

    private void SpawnBolt(SkeletonKit kit)
    {
        Transform t = kit.GetHandTransform(true);
        GameObject g = Instantiate(boltPrefab, t.position, t.rotation);
        g.GetComponent<Projectile>().speed = projectileSpeed;
    }

    private IEnumerator FireBolt(SkeletonKit kit)
    {
        bool fired = false;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / .1f;

            int spriteVal = (int)(t * 3);

            if(spriteVal == 2 && !fired)
            {
                SpawnBolt(kit);
                fired = true;
            }


            kit.SetMainSprite(crossbowSprites[(int)(6 + spriteVal)]);
            yield return new WaitForEndOfFrame();
        }
    }
        private IEnumerator Reload(SkeletonKit kit)
    {
        float t = 0;
        while(t < 1 && reloading)
        {
            t += Time.deltaTime / reloadTime;

            kit.SetMainSprite(crossbowSprites[(int)(t * 5)]);

            yield return new WaitForEndOfFrame();
        }
        if(reloading)
        {
            loaded = true;
            Debug.Log("Reloaded");
        }
    }

    public override void MainRelease(EquipmentManager manager)
    {
        reloading = false;
    }

    public override void SecondaryAction(EquipmentManager manager)
    {
        if (manager.skeletonKit.idle && !reloading)
        {
            manager.skeletonKit.SetSecondarySprite(null);
            manager.skeletonKit.SimpleMelee(damage, reach, swingTime, offHand, hitMask, backSlash, true);
            backSlash = !backSlash;
        }
    }

    public override void SecondaryRelease(EquipmentManager manager)
    {
        //no release action
    }
}
