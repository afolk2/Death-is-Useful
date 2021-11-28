using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinionHealthUI : MonoBehaviour
{
    private CanvasGroup cg;
    private Image healthImage;
    private TextMeshProUGUI healthNum;
    // Start is called before the first frame update
    public void SetupUI(float currentHP)
    {
        cg = transform.GetComponent<CanvasGroup>();
        healthImage = transform.GetComponentInChildren<Image>();
        healthNum = transform.GetComponentInChildren<TextMeshProUGUI>();

        cg.alpha = 0f;
        healthImage.fillAmount = 1f;
        healthNum.text = currentHP.ToString();
    }

    public void SetUIValue(float hpNum, float ratio)
    {
        healthNum.text = hpNum.ToString();
        StopAllCoroutines();
        StartCoroutine(UpdateHealth(ratio));
    }

    private IEnumerator UpdateHealth(float ratio)
    {
        LeanTween.alphaCanvas(cg, 1f, .1f);
        yield return new WaitForSeconds(.1f);
        
        float startFill = healthImage.fillAmount;
        float t = 0;
        while (t < 1)
        {
            healthImage.fillAmount = Mathf.Lerp(startFill, ratio, t);
            t += Time.deltaTime / .7f;

            yield return new WaitForEndOfFrame();
        }
        healthImage.fillAmount = ratio;

        while (t > 0)
        {
            cg.alpha = t;
            t -= Time.deltaTime / 2;

            yield return new WaitForEndOfFrame();
        }
    }
}
