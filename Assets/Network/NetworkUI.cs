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

    private bool isHost;

    [SerializeField] private Transform[] camerasPos;
    public Camera mainCamera;

    [Header("Start of match")]
    [SerializeField] private GameObject netRoomsWindow;
     private CanvasGroup canvasNet;
    [SerializeField] private GameObject jokenpoCanvas;

    [Header("Game")]

    [SerializeField] private GameObject endPanel;

    public int cardCount = 0;
    public GameObject endTurnB;
    public GameObject[] cardIndex;

    [SynchronizableField] public List<int> host_slotCards = new();
    [SynchronizableField] public List<int> guest_slotCards = new();

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

        canvasNet = netRoomsWindow.GetComponent<CanvasGroup>();

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
        canvasNet.alpha = 0f;
        canvasNet.blocksRaycasts = false;

        isHost = (Multiplayer.CurrentRoom.GetUserCount() == 2) ? false : true;

        if (isHost)
        {
            mainCamera.transform.position = camerasPos[0].position;
            mainCamera.transform.rotation = camerasPos[0].rotation;
        }
        else
        {
            mainCamera.transform.position = camerasPos[1].position;
            mainCamera.transform.rotation = camerasPos[1].rotation;
        }
        NetworkGM.instance.RoomJoined();
    }

    void ActivateCard()
    {
        cardIndex[6].GetComponent<Button>().interactable = true;
        NetworkGM.instance.OnFirstTurnEnd -= ActivateCard;
    }

    public void GetCardIndex(int index)
    {
        List<int> slotcards;

        slotcards = (isHost) ? host_slotCards : guest_slotCards;

        reproduce.PlayOneShot(selectCard);
        if (NetworkGM.instance.actionTurn && cardCount < 3)
        {
            if (index < 7)
            {
                //cardIndex[index].SetActive(false);
                cardIndex[index].GetComponent<Button>().interactable = false;
            }
            NetworkGM.instance.FillSlot(index, isHost);

            BroadcastRemoteMethod("AddSlotcard", index, isHost);

            cardCount++;
            endTurnB.SetActive(_ = (cardCount > 2));
        }
    }
    [SynchronizableMethod]
    private void AddSlotcard(int cardindex, bool sentByHost)
    {
        if (sentByHost)
        {
            host_slotCards.Add(cardindex);
        }
        else
        {
            guest_slotCards.Add(cardindex);
        }
    }

    public void BackCard()
    {
        if (NetworkGM.instance.actionTurn && cardCount > 0)
        {
            reproduce.PlayOneShot(UIclick);

            List<int> slotcards;

            slotcards = (isHost) ? host_slotCards : guest_slotCards;

            int slotValue;

            if (isHost)
            {
                slotValue = slotcards.Count;
            }
            else
            {
                slotValue = slotcards.Count + 3;
            }

            if (slotcards.Any())
            {
                int lastUIIndex = slotcards.Last();

                ReturnCard(lastUIIndex);
                BroadcastRemoteMethod("BackCardNet", slotValue - 1, isHost);

            }


            cardCount--;
            endTurnB.SetActive(_ = (cardCount > 2));
        }
    }
    [SynchronizableMethod]
    private void BackCardNet(int slotValue, bool sentByHost)
    {
        GameObject lastCardIndex = NetworkGM.instance.cardSlots[slotValue];

        if (lastCardIndex.transform.childCount > 0)
        {
            StartCoroutine(lastCardIndex.transform.GetComponentInChildren<Card>().DestroySequence());
        }

        if (sentByHost)
        {
            host_slotCards.RemoveAt(host_slotCards.Count - 1);
        }
        else
        {
            guest_slotCards.RemoveAt(guest_slotCards.Count - 1);
        }
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
