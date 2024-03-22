using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Alteruna;

public class NetworkUI : AttributesSync
{
    public static NetworkUI instance;

    private bool imEnemy;

    [SerializeField] private Transform[] camerasPos;
    [SerializeField] private Camera mainCamera;

    [Header("Start of match")]
    [SerializeField] private GameObject netRoomsWindow;
    [SerializeField] private GameObject jokenpoCanvas;

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
    [SerializeField] private AudioClip selectCard;
    [SerializeField] private AudioClip UIclick;
    private AudioSource reproduce;

    public event Action OnHidePanel;
    public event Action OnShowPanel;

    private void Start()
    {
        NetworkGM.instance.OnFirstTurnEnd += ActivateCard;
        NetworkGM.instance.OnShowTurnEnd += BackCard;
        netRoomsWindow.GetComponent<RoomMenu>().OnRoomCreated += RoomJoined;

        endPanel.SetActive(false);
    }

    void Awake()
    {
        instance = this;
        endTurnB = GameObject.Find("EndTurnB");
        endTurnB.SetActive(false);
        reproduce = GetComponent<AudioSource>();
        cardIndex[6].GetComponent<Button>().interactable = false;

        StartCoroutine(WaitCameraAnim());

        //OnHidePanel?.Invoke();
        //OnShowPanel?.Invoke();

        jokenpoCanvas.SetActive(false); //change to true later

    }
    
    private IEnumerator WaitCameraAnim()
    {
        OnHidePanel?.Invoke();
        yield return new WaitForSeconds(0.5f);

        OnShowPanel?.Invoke();
        yield return new WaitForSeconds(1f);

        //jokenpoCanvas.SetActive(true);
    }
    
    void RoomJoined()
    {
        netRoomsWindow.SetActive(false);

        imEnemy = (Multiplayer.CurrentRoom.GetUserCount() == 2) ? true : false;

        if (!imEnemy)
        {
            mainCamera.transform.position = camerasPos[0].position;
            mainCamera.transform.rotation = camerasPos[0].rotation;
        }
        else
        {
            mainCamera.transform.position = camerasPos[1].position;
            mainCamera.transform.rotation = camerasPos[1].rotation;
        }
    }

    void ActivateCard()
    {
        cardIndex[6].GetComponent<Button>().interactable = true;
        GameManager.instance.OnFirstTurnEnd -= ActivateCard;
    }

    public void GetCardIndex(int index)
    {
        reproduce.PlayOneShot(selectCard);
        if (GameManager.instance.actionTurn && cardCount < 3)
        {
            slotCards.Add(index);
            if (index < 7)
            {
                //cardIndex[index].SetActive(false);
                cardIndex[index].GetComponent<Button>().interactable = false;
            }
        }
    }

    public void BackCard()
    {
        reproduce.PlayOneShot(UIclick);
        if (slotCards.Any())
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

    private new void OnDestroy()
    {
        NetworkGM.instance.OnFirstTurnEnd -= ActivateCard;
        NetworkGM.instance.OnShowTurnEnd -= BackCard;
    }
}
