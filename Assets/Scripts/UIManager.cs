using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject endPanel;
    public GameObject[] announce;

    public int cardCount;
    private GameObject endTurnB;
    public GameObject[] cardIndex;

    public List<int> slotCards = new();

    [Header("Sound FX")]
    public AudioClip youWinS;
    public AudioClip youLoseS;
    private AudioSource reproduce;

    void Awake()
    {
        instance = this;
        endTurnB = GameObject.Find("EndTurnB");
        reproduce = GetComponent<AudioSource>();
    }
    public void GetCardIndex(int index)
    {
        if (GameManager.instance.actionTurn && cardCount < 3)
        {
            slotCards.Add(index);
            cardIndex[index].SetActive(false);
        }

    }
    void Update()
    {
        endTurnB.SetActive(_ = (cardCount > 2));
    }

    public void BackCard()
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

        //if (cardCount > 2)
        {
        //    ReturnCard(slot3card, GameManager.instance.cardSlot3, false);
        //    cardCount--;
        }
       // else if (cardCount == 2)
        //{
        //    ReturnCard(slot2card, GameManager.instance.cardSlot2, false);
        //    GameManager.instance.cardCount--;
       // }
       // else if (GameManager.instance.cardCount == 1)
       // {
         //   ReturnCard(slot1card, GameManager.instance.cardSlot1, false);
        //    GameManager.instance.cardCount--;
      //  }
       // else
       // {
        //    return;
       // }
    }

    public void ReturnCard(int cardNumber, GameObject cardSlot, bool isEnemy)
    {
        if (!isEnemy)
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