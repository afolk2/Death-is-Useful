using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
/// <summary>
/// Manages the process of Equiping and unequiping both kits and trinkets. Then pass the new item data along to other classes like SkeletonKit
/// </summary>
public class EquipmentManager : MonoBehaviour
{
    public SkeletonKit skeletonKit;
    public SkeletonAim skeletonAim;

    [SerializeField]
    public Kit Kit
    {
        get { return currentKit; }

        set
        {
            EquipKit(value);
        }
    }

    private Kit currentKit;
    private Trinket currentTrinket;
    
#if UNITY_EDITOR
    [Header("--- Testing ---")]
    public Kit testItem;
#endif
    private void Start()
    {
        skeletonKit = GetComponent<SkeletonKit>();
        skeletonAim = GetComponentInParent<SkeletonAim>();
    }
    public void UseMain()
    {
        if (currentKit != null)
            currentKit.MainAction(this);
    }

    public void ReleaseMain()
    {
        if (currentKit != null)
            currentKit.MainRelease(this);
    }

    public void UseOff()
    {
        if (currentKit != null)
            currentKit.SecondaryAction(this);
    }

    public void ReleaseOff()
    {
        if (currentKit != null)
            currentKit.SecondaryRelease(this);
    }

    public void EquipKit(Kit newKit)
    {
        if (currentKit != null)
        {
            //Drop current kit item
        }

        if (newKit != null)
        {
            //Equip new kit
            currentKit = newKit;
            skeletonKit.UpdateActiveKit(currentKit);
        }
        else
        {
            //Setting kit to null. Unarmed??
            currentKit = null;
            skeletonKit.UpdateActiveKit(null);
        }
    }



    public void EquipTrinket(Trinket newTrinket)
    {
        if (currentTrinket != null)
        {
            //Drop current Trinket
        }

        if (newTrinket != null)
        {
            //Equip new Trinket

        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(EquipmentManager))]
public class EquimentEditor : Editor
{
    
    EquipmentManager manager;
    private void OnEnable()
    {
        manager = (EquipmentManager)target;
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.BeginVertical();
        EditorGUI.BeginChangeCheck();

        if (GUILayout.Button("Equip Test Item"))
        {
            Undo.RecordObject(manager, "Test Item");
            manager.EquipKit(manager.testItem);
        }

        EditorGUI.EndChangeCheck();
        GUILayout.EndVertical();
    }
}

#endif