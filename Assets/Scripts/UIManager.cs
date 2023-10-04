using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject endPanel;
    public GameObject[] announce;

    public int cardCount;
    private GameObject endTurnB;
    public GameObject[] cardIndex;

    public List<int> slotCards = new();

    public TextMeshProUGUI starCount;

    public TextMeshProUGUI EnemyStarCount;

    [Header("Sound FX")]
    public AudioClip youWinS;
    public AudioClip youLoseS;
    private AudioSource reproduce;

    private void Start()
    {
        GameManager.instance.OnFirstTurnEnd += ActivateCard;
    }

    void Awake()
    {
        instance = this;
        endTurnB = GameObject.Find("EndTurnB");
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
    void Update()
    {
        endTurnB.SetActive(_ = (cardCount > 2));
        starCount.text = "Stars: " + GameManager.instance.meStars;
        EnemyStarCount.text = "Stars: " + GameManager.instance.enemyStars;
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