using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Star of match")]

    [SerializeField] private GameObject tutArrows;
    [SerializeField] private GameObject jokenpoCanvas;

    [Header("Final Boss")]

    [SerializeField] private GameObject angryACanvas;

    [Header("Game")]

    [SerializeField] private GameObject endPanel;

    public int cardCount = 0;
    public GameObject endTurnB;
    public GameObject[] cardIndex;

    public List<int> slotCards = new();

    public TextMeshProUGUI starCount;

    public TextMeshProUGUI enemyStarCount;

    [Header("Sound FX")]
    [SerializeField] private AudioClip youWinS;
    [SerializeField] private AudioClip youLoseS;
    private AudioSource reproduce;

    public event Action OnHidePanel;
    public event Action OnShowPanel;

    private void Start()
    {
        GameManager.instance.OnFirstTurnEnd += ActivateCard;
        GameManager.instance.OnShowTurnEnd += BackCard;

        endPanel.SetActive(false);
        jokenpoCanvas.SetActive(false);

        if (ScoreManager.instance._enemyName != Enemy.Name.AngryAna)
        {
            angryACanvas.SetActive(false);
        }
        else
        {
            angryACanvas.SetActive(true);
        }
    }

    public void SpeedTime(bool isFast)
    {
        Time.timeScale = (isFast) ? 2 : 1;  
    }

    void Awake()
    {
        instance = this;
        endTurnB = GameObject.Find("EndTurnB");
        endTurnB.SetActive(false);
        reproduce = GetComponent<AudioSource>();
        cardIndex[6].GetComponent<Button>().interactable = false;

        StartCoroutine(WaitCameraAnim());
    }
    private IEnumerator WaitCameraAnim()
    {
        OnHidePanel?.Invoke();
        yield return new WaitForSeconds(3);

        OnShowPanel?.Invoke();
        yield return new WaitForSeconds(1.5f);

        if (Settings.isFirstTimePlaying)
        {
            tutArrows.SetActive(true);
        }
        else
        {
            tutArrows.SetActive(false);
        }
        jokenpoCanvas.SetActive(true);
    }

    void ActivateCard()
    {
        cardIndex[6].GetComponent<Button>().interactable = true;
        GameManager.instance.OnFirstTurnEnd -= ActivateCard;
    }

    public void GetCardIndex(int index)
    {
        if (GameManager.instance.actionTurn && cardCount < 3)
        {
            slotCards.Add(index);
            if(index < 7)
            {
                //cardIndex[index].SetActive(false);
                cardIndex[index].GetComponent<Button>().interactable = false;
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
        endTurnB.SetActive(_ = (cardCount > 2));
    }

    public void ReturnCard(int cardNumber, GameObject cardSlot, bool isEnemy)
    {
        if (!isEnemy && cardNumber < 7)
        {
            //cardIndex[cardNumber].SetActive(true);
            cardIndex[cardNumber].GetComponent<Button>().interactable = true;
        }
        Debug.Log("A carta " + cardNumber + " retornou!");

        if (cardSlot.transform.childCount > 0)
        {
            StartCoroutine(cardSlot.transform.GetComponentInChildren<Card>().DestroySequence());
            //Destroy(cardSlot.transform.GetChild(0).gameObject);
        }
    }

    public void ReturnCard(int cardNumber)
    {
        if (cardNumber < 7)
        {
            cardIndex[cardNumber].GetComponent<Button>().interactable = true;
        }
        Debug.Log("A carta " + cardNumber + " retornou!");
    }

    public void GameOver(bool enemyWins)
    {
        endPanel.SetActive(true);
        endPanel.GetComponent<Animator>().SetTrigger("End");

        GameManager.instance.GetEnemyAI();

        if (!enemyWins)
        {
            reproduce.PlayOneShot(youWinS);
        }
        else
        {
            reproduce.PlayOneShot(youLoseS);
        }
        endPanel.GetComponent<EndGamePanel>().GameResult(enemyWins);
    }

    private void OnDestroy()
    {
        GameManager.instance.OnFirstTurnEnd -= ActivateCard;
        GameManager.instance.OnShowTurnEnd -= BackCard;
    }
}