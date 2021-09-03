using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class SummonableSkeleton : MonoBehaviour
{

    #region Scroll Summoning UI
    private CanvasGroup scrollGroup;
    private RectTransform scrollPageBG;
    private RectTransform topRoll, botRoll;
    private CanvasGroup dataGroup;
    private TextMeshProUGUI corpseNumText;
    private TextMeshProUGUI costNumText;
    private Image kitImage;
    private Image trinketImage;
    #endregion

    #region Summoning UI Effects
    private Light2D scrollGlow;
    private Light2D summonLight;
    private ParticleSystem summonParticles;
    #endregion

    #region Summon Effects
    [SerializeField] private Sprite[] skellyAnim;
    [SerializeField] private ParticleSystem finishSummonEffect;
    #endregion

    private SpriteRenderer corpseSpriteRenderer;
    private bool summoned;

    [SerializeField] private float entryTime, exitTime, summonTime;

    [Header("Skeleton Values")]
    [SerializeField] private GameObject skellyPrefab;
    public Kit kit;
    public Trinket trinket;
    public int deathNum;
    public int cost;


    public void AssignValues(Kit kit, Trinket trinket, int deathNum, int cost)
    {
        this.kit = kit;
        this.trinket = trinket;
        this.deathNum = deathNum;
        this.cost = cost;

        Start();
    }

    private void Start()
    {
        #region Set up Variables and set entry sequence to default values
        corpseSpriteRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();

        scrollGroup = transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<CanvasGroup>();

        scrollPageBG = scrollGroup.transform.GetChild(0).GetComponent<RectTransform>();
        topRoll = scrollGroup.transform.GetChild(1).GetComponent<RectTransform>();
        botRoll = scrollGroup.transform.GetChild(2).GetComponent<RectTransform>();

        dataGroup = scrollPageBG.transform.GetChild(0).GetComponent<CanvasGroup>();
        scrollGlow = scrollPageBG.transform.GetChild(1).GetComponent<Light2D>();

        corpseNumText = dataGroup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        costNumText = dataGroup.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        kitImage = dataGroup.transform.GetChild(2).GetComponent<Image>();
        trinketImage = dataGroup.transform.GetChild(3).GetComponent<Image>();

        summonLight = transform.GetChild(3).GetComponent<Light2D>();
        summonParticles = transform.GetChild(4).GetComponent<ParticleSystem>();


        dataGroup.alpha = 0;

        scrollPageBG.transform.localScale.Set(1, 0, 1);
        topRoll.anchoredPosition.Set(0, 0);
        botRoll.anchoredPosition.Set(0, 0);

        scrollGroup.transform.localScale = Vector3.zero;
        scrollGroup.alpha = 0;

        summonLight.pointLightInnerAngle = 0;
        summonLight.pointLightOuterRadius = 0;

        summonParticles.Pause();

        #endregion
        #region Set UI values to match kit and trinket
        corpseNumText.text = RomanNumeral.ToRoman(deathNum);
        costNumText.text = cost.ToString();

        if (kit != null)
        {
            kitImage.enabled = true;
            kitImage.sprite = kit.dropSprite;
        }
        else
        {
            kitImage.enabled = false;
            kitImage.sprite = null;
        }

        if (trinket != null)
        {
            trinketImage.enabled = true;
            trinketImage.sprite = trinket.dropSprite;
        }
        else
        {
            trinketImage.enabled = false;
            trinketImage.sprite = null;
        }
        #endregion

        summoned = false;
    }

    public void Spawn()
    {
        summoned = true;
        StartCoroutine(AnimateSummonLighting());
        StartCoroutine(AnimateSummonSkeleton());
    }

    private void OpenSummonMenu()
    {
        if (!summoned)
            StartCoroutine(OpenSequence());
    }

    private void CloseSummonMenu()
    {
        if (!summoned)
            StartCoroutine(CloseSequence());
    }


    private IEnumerator AnimateSummonLighting()
    {
        LeanTween.cancelAll();

        LeanTween.alphaCanvas(dataGroup, 0, summonTime / 4);
        LeanTween.moveLocalY(topRoll.gameObject, .1f, summonTime / 4).setEaseOutBounce();
        LeanTween.moveLocalY(botRoll.gameObject, -.1f, summonTime / 4).setEaseOutBounce();
        LeanTween.scaleY(scrollPageBG.gameObject, 0, summonTime / 4).setEaseOutBounce();

        yield return new WaitForSeconds(summonTime / 4);

        

        LeanTween.scale(scrollGroup.gameObject, Vector2.zero, summonTime / 4).setEaseOutBounce();
        LeanTween.alphaCanvas(scrollGroup, 0, summonTime / 4).setEaseOutBounce();

        scrollGroup.alpha = 0;

        float t = 0;
        while (t < 1)
        {
            summonLight.pointLightOuterAngle = Mathf.Lerp(summonLight.pointLightOuterAngle, 90f, t);
            summonLight.pointLightInnerAngle = Mathf.Min(Mathf.Lerp(summonLight.pointLightInnerAngle, 15, t), summonLight.pointLightOuterAngle);
            summonLight.pointLightOuterRadius = Mathf.Lerp(summonLight.pointLightOuterRadius, 4, t);
            summonLight.pointLightInnerRadius = Mathf.Min(Mathf.Lerp(summonLight.pointLightInnerRadius, 0, t), summonLight.pointLightOuterRadius);
            t += Time.deltaTime / (summonTime * .75f);

            yield return new WaitForEndOfFrame();
        }

        summonLight.pointLightInnerAngle = 45;
        summonLight.pointLightOuterAngle = 90;
        summonLight.pointLightOuterRadius = 4;
        summonLight.pointLightInnerRadius = 0;

        t = 0;
        while (t < 1)
        {
            summonLight.pointLightOuterAngle = Mathf.Lerp(90, 0, t);
            summonLight.pointLightOuterRadius = Mathf.Min(Mathf.Lerp(4, 0, t), summonLight.pointLightInnerRadius);
            summonLight.pointLightInnerAngle = Mathf.Min(Mathf.Lerp(15, 0, t), summonLight.pointLightOuterAngle);
            t += Time.deltaTime / (summonTime * .1f);

            yield return new WaitForEndOfFrame();
        }
        summonLight.pointLightInnerAngle = 0;
        summonLight.pointLightOuterAngle = 0;
        summonLight.pointLightOuterRadius = 0;
        summonParticles.Stop();

        Destroy(this);
    }

    private IEnumerator AnimateSummonSkeleton()
    {
        LeanTween.scale(corpseSpriteRenderer.gameObject, Vector3.one * 1.5f, summonTime).setEaseInCirc();

        for (int i = 0; i < skellyAnim.Length; i++)
        {
            float t = 0;
            Vector2 startPos = corpseSpriteRenderer.transform.position;

            while (t < 1)
            {
                if (i > skellyAnim.Length / 2)
                    corpseSpriteRenderer.transform.position = new Vector2(startPos.x + (Mathf.Sin(Time.time * 20f) * .1f), startPos.y + (Mathf.Sin(Time.time * 20) * .1f));

                t += Time.deltaTime / (summonTime / skellyAnim.Length);
                yield return new WaitForEndOfFrame();
            }
            corpseSpriteRenderer.sprite = skellyAnim[i];
        }

        finishSummonEffect.Play();
        corpseSpriteRenderer.enabled = false;

        FinishSummon();

        Destroy(gameObject, summonTime);
    }


    private void FinishSummon()
    {
        GameObject s = Instantiate(skellyPrefab, transform.position, Quaternion.identity);
        EquipmentManager em = s.transform.GetComponentInChildren<EquipmentManager>();

        em.EquipKit(kit);
        em.EquipTrinket(trinket);


        MinionManager.settings.AddMinion(s.GetComponent<MinionController>());
    }

    private IEnumerator OpenSequence()
    {
        LeanTween.cancel(gameObject);
        scrollGroup.transform.localRotation = Quaternion.Euler(0, 0, 2);
        LeanTween.rotateZ(scrollGroup.gameObject, -2, 1f).setLoopPingPong();

        summonParticles.Play();
        LeanTween.scale(scrollGroup.gameObject, Vector2.one, entryTime / 2).setEaseInBounce();
        LeanTween.alphaCanvas(scrollGroup, 1, entryTime / 2).setEaseInBounce();

        float t = 0;
        while (t < 1)
        {
            
            summonLight.pointLightOuterAngle = Mathf.Lerp(0, 120f, t);
            summonLight.pointLightInnerAngle = Mathf.Min(Mathf.Lerp(0, 100, t), summonLight.pointLightOuterAngle);
            summonLight.pointLightOuterRadius = Mathf.Lerp(0, 3, t);
            t += Time.deltaTime / (entryTime / 2);
            yield return new WaitForEndOfFrame();
        }
        summonLight.pointLightInnerAngle = 100;
        summonLight.pointLightOuterAngle = 120;
        summonLight.pointLightOuterRadius = 3;

        LeanTween.alphaCanvas(dataGroup, 1, entryTime / 2);
        LeanTween.moveLocalY(topRoll.gameObject, 0.8f, entryTime / 2).setEaseInBounce();
        LeanTween.moveLocalY(botRoll.gameObject, -.8f, entryTime / 2).setEaseInBounce();
        LeanTween.scaleY(scrollPageBG.gameObject, 1, entryTime / 2).setEaseInBounce();


    }

    private IEnumerator CloseSequence()
    {

        summonParticles.Stop();

        LeanTween.cancel(gameObject);

        LeanTween.alphaCanvas(dataGroup, 0, exitTime / 2);
        LeanTween.moveLocalY(topRoll.gameObject, .1f, exitTime / 2).setEaseOutBounce();
        LeanTween.moveLocalY(botRoll.gameObject, -.1f, exitTime / 2).setEaseOutBounce();
        LeanTween.scaleY(scrollPageBG.gameObject, 0, exitTime / 2).setEaseOutBounce();

        yield return new WaitForSeconds(exitTime / 2);

        LeanTween.scale(scrollGroup.gameObject, Vector2.zero, exitTime / 2).setEaseOutBounce();
        LeanTween.alphaCanvas(scrollGroup, 0, exitTime / 2).setEaseOutBounce();

        float t = 0;
        while (t < 1)
        {
            summonLight.pointLightOuterAngle = Mathf.Lerp(summonLight.pointLightOuterAngle, 30f, t);
            summonLight.pointLightInnerAngle = Mathf.Min(Mathf.Lerp(100, 0, t), summonLight.pointLightOuterAngle);
            summonLight.pointLightOuterRadius = Mathf.Lerp(3, 0, t);
            t += Time.deltaTime / (exitTime / 2);

            yield return new WaitForEndOfFrame();
        }
        summonLight.pointLightInnerAngle = 0;
        summonLight.pointLightOuterRadius = 0;


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            OpenSummonMenu();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            CloseSummonMenu();
        }
    }
}
