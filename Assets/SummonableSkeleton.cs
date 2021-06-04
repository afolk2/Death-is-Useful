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

    #region Summoning Effects
    private Light2D scrollGlow;
    private Light2D summonLight;
    private ParticleSystem summonParticles;
    #endregion

    private SpriteRenderer corpseSpriteRenderer;
    private bool open;

    [SerializeField] private float entryTime, exitTime;

    [Header("Skeleton Values")]
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
        kitImage.enabled = kit != null;
        kitImage.sprite = kit.dropSprite;
        trinketImage.enabled = trinket != null;
        trinketImage.sprite = trinket.dropSprite;
        #endregion

        
    }

    public void Spawn()
    {

    }

    private void OpenSummonMenu()
    {
        StartCoroutine(OpenSequence());
        
    }

    private void CloseSummonMenu()
    {
        StartCoroutine(CloseSequence());
    }

    private IEnumerator OpenSequence()
    {
        LeanTween.cancelAll();
        scrollGroup.transform.localRotation = Quaternion.Euler(0, 0, 2);
        LeanTween.rotateZ(scrollGroup.gameObject, -2, 1f).setLoopPingPong();

        summonParticles.Play();
        LeanTween.scale(scrollGroup.gameObject, Vector2.one, entryTime / 2).setEaseInBounce(); 
        LeanTween.alphaCanvas(scrollGroup, 1, entryTime / 2).setEaseInBounce(); 
        float t = 0;
        while (t < 1)
        {
            summonLight.pointLightInnerAngle = Mathf.Lerp(0, 100, t);
            summonLight.pointLightOuterRadius = Mathf.Lerp(0, 3, t);
            t += Time.deltaTime / (entryTime / 2);
            yield return new WaitForEndOfFrame();
        }
        summonLight.pointLightInnerAngle = 100;
        summonLight.pointLightOuterRadius = 3;

        LeanTween.alphaCanvas(dataGroup, 1, entryTime / 2);
        LeanTween.moveLocalY(topRoll.gameObject, 0.8f, entryTime / 2).setEaseInBounce(); 
        LeanTween.moveLocalY(botRoll.gameObject, -.8f, entryTime / 2).setEaseInBounce();
        LeanTween.scaleY(scrollPageBG.gameObject, 1, entryTime / 2).setEaseInBounce();

        yield return new WaitForSeconds(entryTime / 2);
    }

    private IEnumerator CloseSequence()
    {
        LeanTween.alphaCanvas(dataGroup, 0, entryTime / 2); 
        LeanTween.moveLocalY(topRoll.gameObject, .1f, entryTime / 2).setEaseOutBounce(); 
        LeanTween.moveLocalY(botRoll.gameObject, -.1f, entryTime / 2).setEaseOutBounce();
        LeanTween.scaleY(scrollPageBG.gameObject, 0, entryTime / 2).setEaseOutBounce();

        yield return new WaitForSeconds(entryTime / 2);

        summonParticles.Stop();

        LeanTween.scale(scrollGroup.gameObject, Vector2.zero, entryTime / 2).setEaseOutBounce();
        LeanTween.alphaCanvas(scrollGroup, 0, entryTime / 2).setEaseOutBounce();
        float t = 0;
        while (t < 1)
        {
            summonLight.pointLightInnerAngle = Mathf.Lerp(100, 0, t);
            summonLight.pointLightOuterRadius = Mathf.Lerp(3, 0, t);
            t += Time.deltaTime / (entryTime / 2);

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
