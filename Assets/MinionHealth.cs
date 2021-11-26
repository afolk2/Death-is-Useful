using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class MinionHealth : MonoBehaviour
{
    private int maxHP;
    private int currentHP;
    private bool dead;

    private CanvasGroup cg;
    private Image healthImage;
    private TextMeshProUGUI healthNum;
    private int Health
    { 
        get { return currentHP; }
        set
        {
            currentHP = value;

            StopCoroutine(UpdateHealth());
            StartCoroutine(UpdateHealth());
        }
    }

    private IEnumerator UpdateHealth()
    {
        LeanTween.alphaCanvas(cg, 1f, .1f);
        yield return new WaitForSeconds(.1f);
        healthNum.text = currentHP.ToString();

        float startFill = healthImage.fillAmount;
        float t = 0;
        while (t < 1)
        {
            healthImage.fillAmount = Mathf.Lerp(startFill, (float)currentHP / (float)maxHP, t);
            t += Time.deltaTime / 2f;

            yield return new WaitForEndOfFrame();
        }
        healthImage.fillAmount = currentHP / maxHP;

        yield return new WaitForSeconds(1f);

        LeanTween.alphaCanvas(cg, 0f, .5f);

    }


    MinionManager mm;

    void Start()
    {
        mm = MinionManager.manager;
        maxHP = mm.minionHP;
        currentHP = maxHP;
        dead = false;

        cg = transform.GetChild(4).GetComponentInChildren<CanvasGroup>();
        healthImage = transform.GetChild(4).GetComponentInChildren<Image>();
        healthNum = transform.GetChild(4).GetComponentInChildren<TextMeshProUGUI>();

        cg.alpha = 0f;
        healthImage.fillAmount = 1f;
        healthNum.text = currentHP.ToString();
    }


    public void Damage(int amount)
    {
        int newHealth = currentHP - amount;

        if (newHealth <= 0)
            Die();
        else
            Health = newHealth;
    }
    public void Heal(int amount)
    {
        int newHealth = currentHP + amount;

        if (newHealth > maxHP)
            currentHP = maxHP;

        Health = newHealth;
    }

    public void Die()
    {
        Health = 0;
        dead = true;
        mm.RemoveMinion(GetComponent<MinionController>());
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