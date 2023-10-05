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
    private EnemyAI enemyAI;

    public bool actionTurn;
    public bool showFase1;
    public bool installEnd = false;

    public GameObject cardFlipped;

    public List<GameObject> cardSlots;

    public GameObject[] FieldcardIndex;

    public int meStars = 0;

    public int enemyStars = 0;

    [Header("Sound FX")]
    public AudioClip flipCard;
    private AudioSource reproduce;

    [Header("Animations")]
    [SerializeField] Animator blackPanel;

    void Awake()
    {
        instance = this;
        Time.timeScale = 1;
        UIManager.instance.cardCount = 0;
        actionTurn = true;
        showFase1 = true;
        showTurnAction = GetComponent<ShowTurnAction>();
        enemyAI = GetComponent<EnemyAI>();
        reproduce = GetComponent<AudioSource>();
        blackPanel.Play("BlackToTrans");
    }

    public event Action OnFirstTurnEnd;
    void FirstTurnEnd() => OnFirstTurnEnd?.Invoke();

    public event Action OnShowTurnEnd;
    void ShowTurnEnd() => OnShowTurnEnd?.Invoke();

    public void InstantiateInSlot(GameObject cardSlot, int index)
    {
        Quaternion newRotation = transform.rotation * Quaternion.Euler(0, 180, 180);
        Instantiate(FieldcardIndex[index], cardSlot.transform.position, newRotation, cardSlot.transform);
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
            return;
        }
    }

    public void EndTurn()
    {
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
        float timeToWait = 1.2f;
        float waitToFlip = 0.15f;

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
                }

                yield return new WaitForSeconds(timeToWait);

                // action
                showTurnAction.ActionInShowTurn(false, i + 1);

                StartCoroutine(WaitInstallAnimation(timeToWait));

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
                }

                yield return new WaitForSeconds(timeToWait);

                // action
                showTurnAction.ActionInShowTurn(true, i + 1);

                StartCoroutine(WaitInstallAnimation(timeToWait));

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

        EndOfShowTurn();
    }

    private IEnumerator ShowTurn2()
    {
        float timeToWait = 1.2f;
        float waitToFlip = 0.15f;

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
                }

                yield return new WaitForSeconds(timeToWait);

                // action
                showTurnAction.ActionInShowTurn(false, i + 1);

                StartCoroutine(WaitInstallAnimation(timeToWait));

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
                }

                yield return new WaitForSeconds(timeToWait);

                // action
                showTurnAction.ActionInShowTurn(true, i + 1);

                StartCoroutine(WaitInstallAnimation(timeToWait));

                i -= 3;
            }
        }

        // wait
        yield return new WaitForSeconds(timeToWait);

        EndOfShowTurn();
    }

    private void EndOfShowTurn()
    {
        // reset slots
        ResetSlots();

        // reset action turn
        actionTurn = true;
        showFase1 = false;
        ShowTurnEnd();
        enemyAI.EnemyPlay();
        meStars += 2;
        enemyStars += 2;
    }

    private IEnumerator WaitInstallAnimation(float t)
    {
        yield return new WaitForSeconds(t);

        yield return new WaitUntil(() => installEnd == true);

        showTurnAction.cameras[1].SetActive(false);
        showTurnAction.cameras[2].SetActive(false);
    }

    public void ResetSlots()
    {
        UIManager.instance.slotCards = new();
    }
}
