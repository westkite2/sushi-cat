using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    //Summary: Create and manage special items

    private int scrollCount;
    private BackgroundController scriptBackgroundController;
    public GameManager GameManager;
    public GameObject objHpHeart;
    public GameObject objBadFood;
    public GameObject objSeaItem;
    public GameObject objCarItem;
    public GameObject objBackRoad;
    

    private void InitializePosition(GameObject objItem)
    {
        //Initialize item position
        objItem.transform.position =
            new Vector2(Random.Range(14.0f, 25.0f), Random.Range(-2.3f, 2.6f));
    }

    IEnumerator ActivateItem()
    {
        //Activate items while game playing

        //Normal mode
        if (!GameManager.isCarMode)
        {
            yield return new WaitForSeconds(3f);
            if (!GameManager.isSeaMode)
            {
                if (!objHpHeart.activeSelf)
                {
                    objHpHeart.SetActive(true);
                }
            }

            yield return new WaitForSeconds(3f);
            if (!GameManager.isSeaMode)
            {
                if (!objBadFood.activeSelf)
                {
                    objBadFood.SetActive(true);
                }
            }
            if ((scriptBackgroundController.scrollCount % 7 == 0) && (scriptBackgroundController.scrollCount <= 35))
            {
                if (!objCarItem.activeSelf)
                {
                    objCarItem.SetActive(true);
                }
            }
            if(scriptBackgroundController.scrollCount % 6 == 0)
            {
                if (!objSeaItem.activeSelf)
                {
                    objSeaItem.SetActive(true);
                }
            }
        }
        //Car mode
        else
        {
            objSeaItem.SetActive(false);

            yield return new WaitForSeconds(1f);
            if (!GameManager.isSeaMode)
            {
                if (!objHpHeart.activeSelf)
                {
                    objHpHeart.SetActive(true);
                }
            }
        }


        StartCoroutine("ActivateItem");
    }

    private void InactivateItem(GameObject FoodObject)
    {
            FoodObject.SetActive(false);
    }

    private void Awake()
    {
        objHpHeart.SetActive(false);
        objBadFood.SetActive(false);
        objSeaItem.SetActive(false);
        objCarItem.SetActive(false);
        InitializePosition(objHpHeart);
        InitializePosition(objBadFood);
        InitializePosition(objSeaItem);
        InitializePosition(objCarItem);
    }

    private void Start()
    {
        scriptBackgroundController = objBackRoad.GetComponent<BackgroundController>();
        Physics2D.IgnoreLayerCollision(3, 3);
        StartCoroutine("ActivateItem");
    }

    private void Update()
    {
        //Clear all items on game clear
        if (GameManager.isGameEnd)
        {
            StopCoroutine("ActivateItem");
            InactivateItem(objHpHeart);
            InactivateItem(objBadFood);
            InactivateItem(objSeaItem);
            InactivateItem(objCarItem);
        }
    }
}
