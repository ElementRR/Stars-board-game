using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private GameObject endTurnB;
    public GameObject[] cardIndex;
    public int slot1card;
    public int slot2card;
    public int slot3card;
    public int slot4card;
    public int slot5card;
    public int slot6card;

    void Awake()
    {
        instance = this;
        endTurnB = GameObject.Find("EndTurnB");
    }
    public void GetCardIndex(int index)
    {
        if (GameManager.instance.actionTurn)
        { 
            if (GameManager.instance.cardCount == 0)
            {
            slot1card = index;
            cardIndex[index].SetActive(false);
            }
            else if (GameManager.instance.cardCount == 1)
            {
            slot2card = index;
            cardIndex[index].SetActive(false);
            }
            else if (GameManager.instance.cardCount == 2)
            {
            slot3card = index;
            cardIndex[index].SetActive(false);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.cardCount > 2)
        {
            endTurnB.SetActive(true);
        }
        else
        {
            endTurnB.SetActive(false);
        }

    }

    public void BackCard()
    {
        if(GameManager.instance.cardCount > 2)
        {
            ReturnCard(slot3card, GameManager.instance.cardSlot3, false);
            GameManager.instance.cardCount--;
        }
        else if (GameManager.instance.cardCount == 2)
        {
            ReturnCard(slot2card, GameManager.instance.cardSlot2, false);
            GameManager.instance.cardCount--;
        }
        else if (GameManager.instance.cardCount == 1)
        {
            ReturnCard(slot1card, GameManager.instance.cardSlot1, false);
            GameManager.instance.cardCount--;
        }
        else
        {
            return;
        }
    }

    public void ReturnCard(int cardNumber, GameObject cardSlot, bool isEnemy)
    {
        if (!isEnemy)
        {
            cardIndex[cardNumber].SetActive(true);
        }
        Debug.Log("A carta " + cardNumber + " retornou!");
        if(cardSlot.transform.childCount > 0)
        {
            Destroy(cardSlot.transform.GetChild(0).gameObject);
        }
    }
}
