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

    [SerializeField] private bool isHost;

    [SerializeField] private Transform[] camerasPos;
    public Camera mainCamera;

    [Header("Start of match")]
    [SerializeField] private GameObject netRoomsWindow;
     private CanvasGroup canvasNet;
    [SerializeField] private GameObject jokenpoCanvas;
    [SerializeField] private TextMeshProUGUI enemyName;

    [Header("Game")]

    [SerializeField] private GameObject endPanel;

    public int cardCount = 0;
    public GameObject endTurnB;
    public GameObject[] cardIndex;

    [SynchronizableField] public List<int> host_slotCards = new();
    [SynchronizableField] public List<int> guest_slotCards = new();
    [SynchronizableField] private int playersWaiting;

    public TextMeshProUGUI starCount;

    public TextMeshProUGUI enemyStarCount;

    [Header("Sound FX")]
    [SerializeField] private AudioClip youWinS;
    [SerializeField] private AudioClip youLoseS;
    [SerializeField] private AudioClip selectCard;
    [SerializeField] private AudioClip UIclick;
    private AudioSource reproduce;

    public event Action OnHidePanel;

    [Header("Time management")]
    private const float timeToChoose = 30;
    private float timeRemain = timeToChoose;
    [SerializeField] private TextMeshProUGUI trText;
    [SerializeField] private TextMeshProUGUI timeText;
    public TextMeshProUGUI waiting;

    private float _count;

    private void Start()
    {
        NetworkGM.instance.OnFirstTurnEnd += ActivateCard;
        NetworkGM.instance.OnShowTurnEnd += ShowTurnEnded;

        Multiplayer.OnRoomJoined.AddListener(RoomJoined);
        Multiplayer.OnConnected.AddListener(CheckOtherPlayer);

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

        //StartCoroutine(WaitCameraAnim());

        OnHidePanel?.Invoke();

        jokenpoCanvas.SetActive(true);

    }

    private void Update()
    {
        if (NetworkGM.instance.actionTurn)
        {
            timeRemain -= Time.deltaTime;
            int time = (int)timeRemain;
            timeText.text = time.ToString();

            if(timeRemain <= 0.0f)
            {
                //fill with stars
                int cardCount;
                cardCount = (isHost) ? host_slotCards.Count : guest_slotCards.Count;

                for (int i = cardCount; i < 3; i++)
                {
                    GetCardIndex(7);
                }

                WaitFase();
            }
        }
    }

    private void CheckOtherPlayer(Multiplayer multiplayer, Endpoint endpoint)
    {
        if (!Multiplayer.InRoom)
        {
            canvasNet.alpha = 1f;
            canvasNet.blocksRaycasts = true;
            enemyName.text = "Player";
        }
    }
    public void WaitFase()
    {  
        NetworkGM.instance.actionTurn = false;
        OnHidePanel?.Invoke();
        //change time to choose
        timeRemain = timeToChoose;
        //deactivate time
        trText.gameObject.SetActive(false);

        //activate waiting
        waiting.gameObject.SetActive(true);

        BroadcastRemoteMethod("WaitFaseNet", isHost);
    }

    [SynchronizableMethod]
    void WaitFaseNet(bool isHost)
    {
        playersWaiting++;
        if(playersWaiting == 2)
        {
            //function endturn in gm
            NetworkGM.instance.EndTurn();
            playersWaiting = 0;
        }
    }

    private IEnumerator WaitCameraAnim()
    {
        OnHidePanel?.Invoke();
        yield return new WaitForSeconds(0.5f);

        //OnShowPanel?.Invoke();
        yield return new WaitForSeconds(1f);

        //jokenpoCanvas.SetActive(true);
    }
    
    void RoomJoined(Multiplayer multiplayer, Room room, User user)
    {
        canvasNet.alpha = 0f;
        canvasNet.blocksRaycasts = false;

        jokenpoCanvas.SetActive(true);

        //Settings settings = GameObject.FindGameObjectWithTag("Settings").GetComponent<Settings>();

        isHost = user.IsHost;

        if (isHost)
        {
            mainCamera.transform.SetPositionAndRotation(camerasPos[0].position, camerasPos[0].rotation);
            OnHidePanel?.Invoke();
            waiting.gameObject.SetActive(true);
            //NetworkGM.instance.meTowerSkins = settings.meTowerSkins;
        }
        else
        {
            enemyName.text = room.Name;
            mainCamera.transform.SetPositionAndRotation(camerasPos[1].position, camerasPos[1].rotation);
            //NetworkGM.instance.enemyTowerSkins = settings.meTowerSkins;

            (enemyStarCount, starCount) = (starCount, enemyStarCount);
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
        reproduce.PlayOneShot(selectCard);
        if (NetworkGM.instance.actionTurn && cardCount < 3)
        {
            if (index < 7)
            {
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

                ReturnCard(lastUIIndex, false);
                BroadcastRemoteMethod("BackCardNet", slotValue - 1, isHost);

            }


            cardCount--;
        }
        endTurnB.SetActive(_ = (cardCount > 2));
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


    public void ReturnCard(int cardNumber, GameObject cardSlot)
    {
        if (cardNumber < 7)
        {
            cardIndex[cardNumber].GetComponent<Button>().interactable = true;
        }
        Debug.Log("A carta " + cardNumber + " retornou!");

        if (cardSlot.transform.childCount > 0)
        {
            StartCoroutine(cardSlot.transform.GetComponentInChildren<Card>().DestroySequence());
        }
    }

    public void ReturnCard(int cardNumber, bool isInhibiton) 
    {
        if (cardNumber < 7 && !isInhibiton)
        {
            cardIndex[cardNumber].GetComponent<Button>().interactable = true;
        }
        else if (cardNumber < 7)
        {
            InvokeRemoteMethod("ReturnOtherCard", UserId.All, cardNumber);
            //BroadcastRemoteMethod("ReturnOtherCard", cardNumber);
        }
    }
    [SynchronizableMethod]
    void ReturnOtherCard(int cardNumber)
    {
        cardIndex[cardNumber].GetComponent<Button>().interactable = true;
        Debug.Log("other card " + cardNumber + "returned");
        
    }

    private void ShowTurnEnded()
    {
        BroadcastRemoteMethod("ResetSlots", isHost);
        cardCount = 0;
        endTurnB.SetActive(_ = (cardCount > 2));
        waiting.gameObject.SetActive(false);
        trText.gameObject.SetActive(true);

        if (isHost)
        {
            enemyName.text = Multiplayer.CurrentRoom.Users.Last().Name;
        }
    }
    [SynchronizableMethod]
    private void ResetSlots(bool isHost)
    {
        if (isHost)
        {
            host_slotCards.Clear();
        }
        else
        {
            guest_slotCards.Clear();
        }
    }

    public void GameOver(bool enemyWins)
    {
        endPanel.SetActive(true);
        endPanel.GetComponent<Animator>().SetTrigger("End");


        switch ((isHost ? 0 : 1) + (enemyWins ? 3 : 0))
        {
            case 0 or 4:
                reproduce.PlayOneShot(youWinS);
                endPanel.GetComponent<EndGamePanel>().GameResult(false);
                break;
            case 1 or 3:
                reproduce.PlayOneShot(youLoseS);
                endPanel.GetComponent<EndGamePanel>().GameResult(true);
                break;
            default:
                Debug.Log("Wrong winner");
                break;
        }

    }

    private new void OnDestroy()
    {
        NetworkGM.instance.OnFirstTurnEnd -= ActivateCard;
        NetworkGM.instance.OnShowTurnEnd -= BackCard;
    }
}
