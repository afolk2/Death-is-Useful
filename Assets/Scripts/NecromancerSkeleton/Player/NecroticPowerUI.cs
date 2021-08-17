using System;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class NecroticPowerUI : MonoBehaviour
{
    [SerializeField] private float increaseTime;

    private Image meterBase;
    private Image redLeft, redRight, blueLeft, blueRight;

    private void Awake()
    {
        meterBase = transform.GetChild(0).GetComponent<Image>();

        redLeft = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>();
        redRight = transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<Image>();

        blueLeft = transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Image>();
        blueRight = transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Image>();

        StartCoroutine(ColorPulse());
    }

    private IEnumerator ColorPulse()
    {
        float t;
        while (true)
        {
            t = 0;
            while (t < 1)
            {
                t += Time.deltaTime / increaseTime;
                blueLeft.color = Color.Lerp(new Color(1, 1, 1, .5f), new Color(1, 1, 1, .2f), t);
                blueRight.color = Color.Lerp(new Color(1, 1, 1, .5f), new Color(1, 1, 1, .2f), t);
                yield return new WaitForEndOfFrame();
            }

            t = 0;
            while (t < 1)
            {
                t += Time.deltaTime / increaseTime;
                blueLeft.color = Color.Lerp(new Color(1, 1, 1, .2f), new Color(1, 1, 1, .5f), t);
                blueRight.color = Color.Lerp(new Color(1, 1, 1, .2f), new Color(1, 1, 1, .5f), t);
                yield return new WaitForEndOfFrame();
            }
        }
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

        while (t < 1)
        {
            t += Time.deltaTime / increaseTime;
            blueLeft.fillAmount = Mathf.Lerp(blueLeft.fillAmount, ratio, t);
            blueRight.fillAmount = Mathf.Lerp(blueLeft.fillAmount, ratio, t);
            yield return new WaitForEndOfFrame();
        }

        blueLeft.fillAmount = ratio;
        blueRight.fillAmount = ratio;
    }
}