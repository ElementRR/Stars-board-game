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

    public GameObject cardFlipped;

    public List<GameObject> cardSlots;

    public GameObject[] FieldcardIndex;

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
            ButtonAnimations.instance.EndActionT();
            if (showFase1)
            {
                StartCoroutine(ShowTurn1());
            }
            else
            {
                StartCoroutine(ShowTurn2());
            }
        }
    }


    private IEnumerator ShowTurn1()
    {
        float timeToWait = 1.7f;
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

                if(i == 5)
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

        // reset slots
        ResetSlots();

        // reset action turn
        actionTurn = true;
        showFase1 = false;
        ButtonAnimations.instance.EndShowT();
        enemyAI.EnemyPlay();
    }

    private IEnumerator ShowTurn2()
    {
        float timeToWait = 1.7f;
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

                i -= 3;
            }
        }

        // wait
        yield return new WaitForSeconds(timeToWait);

        // reset slots
        ResetSlots();

        // reset action turn
        actionTurn = true;
        showFase1 = true;
        ButtonAnimations.instance.EndShowT();
        enemyAI.EnemyPlay();
    }

    public void ResetSlots()
    {
        UIManager.instance.slotCards = new();
    }
}
