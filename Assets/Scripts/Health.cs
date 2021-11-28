using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class Health : MonoBehaviour
{
    protected int maxHP;
    protected int currentHP;
    protected bool dead;

    protected virtual void Start()
    {
        maxHP = 15;
        currentHP = maxHP;
        dead = false;
    }

    protected virtual void SetHealth(int value)
    {
        currentHP = value;
    }
    public virtual void Damage(int amount)
    {
        int newHealth = currentHP - amount;

        if (newHealth <= 0)
            Die();
        else
            SetHealth(newHealth);
    }
    public virtual void Heal(int amount)
    {
        if (currentHP == maxHP)
            return;

        int newHealth = currentHP + amount;

        if (newHealth > maxHP)
            newHealth = maxHP;

        SetHealth(newHealth);
    }
    public virtual void Die()
    {
        SetHealth(0);
        dead = true;    
    }
}
//The health editor method in the health script adds custom functionality to the inspector when selecting a health object. 
#if UNITY_EDITOR
[CustomEditor(typeof(MinionHealth))]
public class HealthEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //DrawDefaultInspector();
        MinionHealth script = (MinionHealth)target;
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Damage"))
        {
            script.Damage(1);
        }
        if (GUILayout.Button("Heal"))
        {
            script.Heal(1);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Damage 5"))
        {
            script.Damage(5);
        }
        if (GUILayout.Button("Heal 5"))
        {
            script.Heal(5);
        }
        GUILayout.EndHorizontal();
    }
}
#endif