using System;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class NecroticPowerUI : MonoBehaviour
{
    [SerializeField] private float increaseTime;

    private Image meterBase;
    private Image redLeft, redRight, blueLeft, blueRight;

    private void Start()
    {
        meterBase = transform.GetChild(0).GetComponent<Image>();

        redLeft = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>();
        redRight = transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<Image>();

        blueLeft = transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Image>();
        blueRight = transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Image>();
    }

    //Manage power bar like health bar with stagger bar values
    public void SetPower(float ratio)
    {
        StartCoroutine(ChangeMeter(ratio));
    }

    private IEnumerator ChangeMeter(float ratio)
    {
        redLeft.fillAmount = ratio;
        redRight.fillAmount = ratio;

        float t = 0;

        while(t < 0)
        {
            t += Time.deltaTime / increaseTime;
            blueLeft.fillAmount = Mathf.Lerp(blueLeft.fillAmount, ratio, t);
            blueRight.fillAmount = Mathf.Lerp(blueLeft.fillAmount, ratio, t);
            yield return new WaitForEndOfFrame();
        }
    }
}