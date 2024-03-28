using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;
using UnityEditor;

public class NetworkGM : AttributesSync
{
    public static NetworkGM instance;

    private ShowTurnAction showTurnAction;

    public bool actionTurn = false;
    // if showFase1 = true, the player starts showing cards
    public bool showFase1;
    public bool installEnd = true;

    public GameObject cardFlipped;

    public List<GameObject> cardSlots;

    public GameObject[] fieldCardIndex;

    private const int endTurnStars = 1;

    public static int meStars = 0;

    public static int enemyStars = 0;

    [Header("First showfase indicator")]
    [SerializeField] private FirstIndicator firstIndicator;

    [Header("Sound FX")]
    public AudioClip flipCard;
    [SerializeField] AudioClip endTurnSound;
    private AudioSource reproduce;

    [Header("Animations")]
    [SerializeField] Animator blackPanel;

    public delegate void Outdoor(string message);
    public static event Outdoor OnMessageSent;

    [Header("Tower skins")]
    [SynchronizableField] public List<int> meTowerSkins = new(new int[] { 0, 0, 0, 0, 0 });
    [SynchronizableField] public List<int> enemyTowerSkins = new(new int[] { 0, 0, 0, 0, 0 });

    void Awake()
    {
        instance = this;
        actionTurn = false;
        showTurnAction = GetComponent<ShowTurnAction>();
        reproduce = GetComponent<AudioSource>();
        blackPanel.Play("BlackToTrans");
    }

    public event Action OnFirstTurnEnd;
    void FirstTurnEnd() => OnFirstTurnEnd?.Invoke();

    public event Action OnShowTurnEnd;
    void ShowTurnEnd() => OnShowTurnEnd?.Invoke();

    public void RoomJoined()
    {
        showTurnAction.cameraLocs[0].position = NetworkUI.instance.mainCamera.transform.position;
        showTurnAction.cameraLocs[0].rotation = NetworkUI.instance.mainCamera.transform.rotation;
        /*
        playersInRoom++;

        if(playersInRoom > 1)
        {
            BroadcastRemoteMethod("StartGame");
        }
        */
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        ShowTurnEnd();
        Debug.Log("Game Started");
        actionTurn = true;
        meStars = 0;
        enemyStars = 0;
        NetworkUI.instance.cardCount = 0;
        NetworkUI.instance.waiting.gameObject.SetActive(false);
    }

    public void InstantiateInSlot(GameObject cardSlot, int index)
    {
        Quaternion newRotation = transform.rotation * Quaternion.Euler(0, 180, 180);
        Instantiate(fieldCardIndex[index], cardSlot.transform.position, newRotation, cardSlot.transform);
    }

    public void FillSlot(int cardIndex, bool isHost)
    {
        BroadcastRemoteMethod("FillSlotNet", cardIndex, isHost);
    }

    [SynchronizableMethod]
    private void FillSlotNet(int cardIndex, bool isHost)
    {
        int slotValue;

        Debug.Log("is Host: " + isHost);

        if (isHost)
        {
            slotValue = NetworkUI.instance.host_slotCards.Count;
        }
        else
        {
            slotValue = NetworkUI.instance.guest_slotCards.Count + 3;
        }

        InstantiateInSlot(cardSlots[slotValue], cardIndex);
    }

    public void EndTurn()
    {
        reproduce.PlayOneShot(endTurnSound);
        NetworkUI.instance.waiting.gameObject.SetActive(false);
        if (NetworkUI.instance.cardCount > 2)
        {
            NetworkUI.instance.cardCount = 0;
            actionTurn = false;
            if (showFase1)
            {
                StartCoroutine(ShowTurn1());
            }
            else
            {
                StartCoroutine(ShowTurn2());
            }
        }
        FirstTurnEnd();
    }

    private IEnumerator ShowTurn1()
    {
        float timeToWait = 1.0f;
        float waitToFlip = 0.19f;

        for (int i = 0; i < 6;)
        {
            if (i < 3)
            {
                yield return new WaitForSeconds(timeToWait);
                // show the first card
                if (cardSlots[i].GetComponentInChildren<Animator>() != null)
                {
                    cardSlots[i].GetComponentInChildren<Animator>().SetTrigger("Flip");
                    yield return new WaitForSeconds(waitToFlip);
                    reproduce.PlayOneShot(flipCard);
                    yield return new WaitForSeconds(waitToFlip);
                    cardSlots[i].GetComponentInChildren<Card>().OnParticleTrigger();
                }

                yield return new WaitForSeconds(timeToWait);

                // action
                showTurnAction.ActionInShowTurn(false, i + 1);

                yield return StartCoroutine(WaitInstallAnimation(timeToWait));

                i += 3;
            }
            else
            {
                yield return new WaitForSeconds(timeToWait);
                // show the first card
                if (cardSlots[i].GetComponentInChildren<Animator>() != null)
                {
                    cardSlots[i].GetComponentInChildren<Animator>().SetTrigger("Flip");
                    yield return new WaitForSeconds(waitToFlip);
                    reproduce.PlayOneShot(flipCard);
                    yield return new WaitForSeconds(waitToFlip);
                    cardSlots[i].GetComponentInChildren<Card>().OnParticleTrigger();
                }

                yield return new WaitForSeconds(timeToWait);

                // action
                showTurnAction.ActionInShowTurn(true, i + 1);

                yield return StartCoroutine(WaitInstallAnimation(timeToWait));

                if (i == 5)
                {
                    break;
                }
                else
                {
                    i -= 2;
                }
            }
        }

        // wait
        yield return new WaitForSeconds(timeToWait);

        EndOfShowTurn(true);
    }

    private IEnumerator ShowTurn2()
    {
        float timeToWait = 1.0f;
        float waitToFlip = 0.19f;

        for (int i = 3; i < 6;)
        {
            if (i < 3)
            {
                yield return new WaitForSeconds(timeToWait);
                // show the first card
                if (cardSlots[i].GetComponentInChildren<Animator>() != null)
                {
                    cardSlots[i].GetComponentInChildren<Animator>().SetTrigger("Flip");
                    yield return new WaitForSeconds(waitToFlip);
                    reproduce.PlayOneShot(flipCard);
                    yield return new WaitForSeconds(waitToFlip);
                    cardSlots[i].GetComponentInChildren<Card>().OnParticleTrigger();
                }

                yield return new WaitForSeconds(timeToWait);

                // action
                showTurnAction.ActionInShowTurn(false, i + 1);

                yield return StartCoroutine(WaitInstallAnimation(timeToWait));

                i += 4;
            }
            else
            {
                yield return new WaitForSeconds(timeToWait);
                // show the first card
                if (cardSlots[i].GetComponentInChildren<Animator>() != null)
                {
                    cardSlots[i].GetComponentInChildren<Animator>().SetTrigger("Flip");
                    yield return new WaitForSeconds(waitToFlip);
                    reproduce.PlayOneShot(flipCard);
                    yield return new WaitForSeconds(waitToFlip);
                    cardSlots[i].GetComponentInChildren<Card>().OnParticleTrigger();
                }

                yield return new WaitForSeconds(timeToWait);

                // action
                showTurnAction.ActionInShowTurn(true, i + 1);

                yield return StartCoroutine(WaitInstallAnimation(timeToWait));

                i -= 3;
            }
        }

        // wait
        yield return new WaitForSeconds(timeToWait);

        EndOfShowTurn(false);
    }

    private void EndOfShowTurn(bool isTurn1)
    {
        // reset action turn
        actionTurn = true;

        showFase1 = !isTurn1;
        firstIndicator.UpdateIndPos(showFase1);
        ShowTurnEnd();
        meStars += endTurnStars;
        NetworkUI.instance.starCount.text = "" + meStars;
        enemyStars += endTurnStars;
        NetworkUI.instance.enemyStarCount.text = "" + enemyStars;
        OnMessageSent?.Invoke("+" + endTurnStars + " star to both players");
    }

    private IEnumerator WaitInstallAnimation(float t)
    {
        yield return new WaitForSeconds(t);

        yield return new WaitUntil(() => installEnd == true);

        //showTurnAction.cameras[1].SetActive(false);
        //showTurnAction.cameras[2].SetActive(false);
        showTurnAction.mainCamera.transform.SetPositionAndRotation(showTurnAction.cameraLocs[0].position,
            showTurnAction.cameraLocs[0].rotation);
    }
}
