using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShowTurnAction))]
[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private ShowTurnAction showTurnAction;
    public EnemyAI enemyAI;
    public static Enemy.Name enemyIndex;

    public bool actionTurn;
    // if showFase1 = true, the player starts showing cards
    public bool showFase1;
    public bool installEnd = true;

    public GameObject cardFlipped;

    public List<GameObject> cardSlots;

    public GameObject[] fieldCardIndex;

    private const int endTurnStars = 1;

    public static int meStars = 0;

    public static int enemyStars = 0;

    [Header("Sound FX")]
    public AudioClip flipCard;
    [SerializeField] AudioClip endTurnSound;
    private AudioSource reproduce;

    [Header("Animations")]
    [SerializeField] Animator blackPanel;

    public delegate void Outdoor(string message);
    public static event Outdoor OnMessageSent;

    void Awake()
    {
        instance = this;
        Time.timeScale = 1;
        UIManager.instance.cardCount = 0;
        meStars = 0;
        enemyStars = 0;
        actionTurn = true;
        showTurnAction = GetComponent<ShowTurnAction>();
        reproduce = GetComponent<AudioSource>();
        blackPanel.Play("BlackToTrans");
        OnFirstTurnEnd += GetEnemyAI;
    }

    public event Action OnFirstTurnEnd;
    void FirstTurnEnd() => OnFirstTurnEnd?.Invoke();

    public event Action OnShowTurnEnd;
    void ShowTurnEnd() => OnShowTurnEnd?.Invoke();

    public void InstantiateInSlot(GameObject cardSlot, int index)
    {
        Quaternion newRotation = transform.rotation * Quaternion.Euler(0, 180, 180);
        Instantiate(fieldCardIndex[index], cardSlot.transform.position, newRotation, cardSlot.transform);
    }

    public void FillSlot(int cardIndex)
    {
        if(actionTurn && UIManager.instance.cardCount < 3)
        {
            InstantiateInSlot(cardSlots[UIManager.instance.cardCount], cardIndex);
            UIManager.instance.cardCount++;
        }
        else
        {
            if(UIManager.instance.cardCount < 3)
            {
                UIManager.instance.cardCount++;
            }
        }

        UIManager.instance.endTurnB.SetActive(_ = (UIManager.instance.cardCount > 2));
    }

    public void EndTurn()
    {
        reproduce.PlayOneShot(endTurnSound);
        if (UIManager.instance.cardCount > 2)
        {
            UIManager.instance.cardCount = 0;
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
            if(i < 3)
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

    public void GetEnemyAI()
    {
        if (GetComponent<EdAI>() != null)
        {
            enemyAI = GetComponent<EdAI>();
            enemyIndex = Enemy.Name.Ed;
        }
        else if (GetComponent<RickAI>() != null)
        {
            enemyAI = GetComponent<RickAI>();
            enemyIndex = Enemy.Name.Rick;
        }
        else if (GetComponent<AnaAI>() != null)
        {
            enemyAI = GetComponent<AnaAI>();
            enemyIndex = Enemy.Name.Ana;
        }
        else if (GetComponent<AngryAnaAI>() != null)
        {
            enemyAI = GetComponent<AngryAnaAI>();
            enemyIndex = Enemy.Name.AngryAna;
        }
        Debug.Log(enemyAI);
    }

    private void EndOfShowTurn(bool isTurn1)
    {
        // reset slots
        ResetSlots();

        // reset action turn
        actionTurn = true;

        showFase1 = !isTurn1;

        ShowTurnEnd();
        meStars += endTurnStars;
        UIManager.instance.starCount.text = "" + meStars;
        enemyStars += endTurnStars;
        UIManager.instance.enemyStarCount.text = "" + enemyStars;
        OnMessageSent?.Invoke("+" + endTurnStars + " star to both players");
        enemyAI.EnemyPlay();
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

    public void ResetSlots()
    {
        UIManager.instance.slotCards = new();
    }

    private void OnDestroy()
    {
        OnFirstTurnEnd -= GetEnemyAI;
    }
}
