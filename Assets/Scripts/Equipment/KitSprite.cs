using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitSprite : MonoBehaviour
{
    private SpriteRenderer mainHand, offHand, mainItem, offItem;
    private int mainItemLayer, offItemLayer;
    private int yLayer;
    // Start is called before the first frame update
    void Start()
    {
        mainHand = transform.GetChild(0).GetComponent<SpriteRenderer>();
        offHand = transform.GetChild(1).GetComponent<SpriteRenderer>();
        mainItem = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
        offItem = transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>();
    }


    public void SetKitLayers(int main, int off)
    {
        mainItemLayer = main;
        offItemLayer = off;
    }

    // Update is called once per frame
    void Update()
    {
        yLayer = Mathf.RoundToInt(transform.parent.position.y * -1000) + 100;
        UpdateSpriteSortLayer();
    }

    private void UpdateSpriteSortLayer()
    {
        mainHand.sortingOrder = yLayer;
        offHand.sortingOrder = yLayer;

        mainItem.sortingOrder = mainHand.sortingOrder + mainItemLayer;
        offItem.sortingOrder = offHand.sortingOrder + offItemLayer;
    }
}
