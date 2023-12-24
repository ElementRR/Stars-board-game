using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Jokenpo")]

    public GameObject jokenpoCanvas;

    [Header("Game")]

    public GameObject endPanel;
    public GameObject[] announce;

    public int cardCount = 0;
    public GameObject endTurnB;
    public GameObject[] cardIndex;

    public List<int> slotCards = new();

    public TextMeshProUGUI starCount;

    public TextMeshProUGUI enemyStarCount;

    [Header("Sound FX")]
    public AudioClip youWinS;
    public AudioClip youLoseS;
    private AudioSource reproduce;

    private void Start()
    {
        GameManager.instance.OnFirstTurnEnd += ActivateCard;

        jokenpoCanvas.SetActive(true);
    }

    public void SpeedTime()
    {
        Time.timeScale = 2;
    }

    void Awake()
    {
        instance = this;
        endTurnB = GameObject.Find("EndTurnB");
        endTurnB.SetActive(false);
        reproduce = GetComponent<AudioSource>();
        cardIndex[6].GetComponent<Button>().interactable = false;
    }

    void ActivateCard()
    {
        cardIndex[6].GetComponent<Button>().interactable = true;
    }

    public void GetCardIndex(int index)
    {
        if (GameManager.instance.actionTurn && cardCount < 3)
        {
            slotCards.Add(index);
            if(index < 7)
            {
                cardIndex[index].SetActive(false);
            }
        }
    }

    public void BackCard()
    {
        if(slotCards.Any())
        {
            int lastUIIndex = slotCards.Last<int>();
            GameObject lastCardIndex = GameManager.instance.cardSlots[cardCount - 1];

            if (GameManager.instance.actionTurn && cardCount > 0)
            {
                ReturnCard(lastUIIndex, lastCardIndex, false);
                slotCards.Remove(slotCards.Last<int>());
                cardCount--;
                endTurnB.SetActive(_ = (cardCount > 2));
            }
            else
            {
                return;
            }
        }
    }

    public void ReturnCard(int cardNumber, GameObject cardSlot, bool isEnemy)
    {
        if (!isEnemy && cardNumber < 7)
        {
            cardIndex[cardNumber].SetActive(true);
        }
        Debug.Log("A carta " + cardNumber + " retornou!");

        if (cardSlot.transform.childCount > 0)
        {
            Destroy(cardSlot.transform.GetChild(0).gameObject);
        }
    }

    public void GameOver(bool enemyWins)
    {
        endPanel.SetActive(true);
        if (!enemyWins)
        {
            reproduce.PlayOneShot(youWinS);
            announce[0].SetActive(true);
            announce[1].SetActive(false);
            ScoreManager.instance.AddScore(100);
        }
        else
        {
            reproduce.PlayOneShot(youLoseS);
            announce[1].SetActive(true);
            announce[0].SetActive(false);
            ScoreManager.instance.AddScore(50);
        }
    }
}