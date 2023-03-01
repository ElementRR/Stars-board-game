using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject endPanel;
    public GameObject[] announce;

    private GameObject endTurnB;
    public GameObject[] cardIndex;
    public int slot1card;
    public int slot2card;
    public int slot3card;
    public int slot4card;
    public int slot5card;
    public int slot6card;

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
        if (GameManager.instance.cardCount > 2)
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
        }
        else
        {
            reproduce.PlayOneShot(youLoseS);
            announce[1].SetActive(true);
            announce[0].SetActive(false);
        }
    }
}