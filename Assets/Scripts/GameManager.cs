using System.Collections;
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

    public int cardCount;
    public GameObject cardFlipped;
    public GameObject cardSlot1;
    public GameObject cardSlot2;
    public GameObject cardSlot3;
    public GameObject cardSlot4;
    public GameObject cardSlot5;
    public GameObject cardSlot6;

    public GameObject[] FieldcardIndex;

    [Header("Sound FX")]
    public AudioClip flipCard;
    private AudioSource reproduce;

    void Awake()
    {
        instance = this;
        cardCount = 0;
        actionTurn = true;
        showFase1 = true;
        showTurnAction = GetComponent<ShowTurnAction>();
        enemyAI = GetComponent<EnemyAI>();
        reproduce = GetComponent<AudioSource>();
    }

    public void InstantiateInSlot(GameObject cardSlot, bool isEnemy, int index)
    {
        Quaternion newRotation = transform.rotation * Quaternion.Euler(0, 180, 180);
        Instantiate(FieldcardIndex[index], cardSlot.transform.position, newRotation, cardSlot.transform);

        if (!isEnemy)
        {
            cardCount++;
        }
    }

    public void FillSlot(int cardIndex)
    {
        if (actionTurn)
        {
            if (actionTurn && cardCount < 3)
            {
                if (cardCount < 1)
                {
                    InstantiateInSlot(cardSlot1, false, cardIndex);
                }
                else if (cardCount < 2)
                {
                    InstantiateInSlot(cardSlot2, false, cardIndex);
                }
                else if (cardCount < 3)
                {
                    InstantiateInSlot(cardSlot3, false, cardIndex);
                }
            }
        }
    }

    public void EndTurn()
    {
        if (cardCount > 2)
        {
            cardCount = 0;
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

        yield return new WaitForSeconds(1.5f);
        // show the first card
        if (cardSlot1.GetComponentInChildren<Animator>() != null) 
        {
            cardSlot1.GetComponentInChildren<Animator>().SetTrigger("Flip");
            yield return new WaitForSeconds(waitToFlip);
            reproduce.PlayOneShot(flipCard);
        }
        
        yield return new WaitForSeconds(timeToWait);

        // action
        showTurnAction.ActionInShowTurn(false, 1);

        // wait
        yield return new WaitForSeconds(1f);

        // show the first card AI
        if (cardSlot4.GetComponentInChildren<Animator>() != null)
        {
            cardSlot4.GetComponentInChildren<Animator>().SetTrigger("Flip");
            yield return new WaitForSeconds(waitToFlip);
            reproduce.PlayOneShot(flipCard);
        }

        yield return new WaitForSeconds(timeToWait);

        // action
        showTurnAction.ActionInShowTurn(true, 4);

        // wait
        yield return new WaitForSeconds(1f);

        // show the second card
        if (cardSlot2.GetComponentInChildren<Animator>() != null)
        {
            cardSlot2.GetComponentInChildren<Animator>().SetTrigger("Flip");
            yield return new WaitForSeconds(waitToFlip);
            reproduce.PlayOneShot(flipCard);
        }

        yield return new WaitForSeconds(timeToWait);

        // action
        showTurnAction.ActionInShowTurn(false, 2);

        // wait
        yield return new WaitForSeconds(1f);
        // show the second card AI
        if (cardSlot5.GetComponentInChildren<Animator>() != null)
        {
            cardSlot5.GetComponentInChildren<Animator>().SetTrigger("Flip");
            yield return new WaitForSeconds(waitToFlip);
            reproduce.PlayOneShot(flipCard);
        }

        yield return new WaitForSeconds(timeToWait);

        // action
        showTurnAction.ActionInShowTurn(true, 5);

        // wait
        yield return new WaitForSeconds(1f);

        // show the third cards
        if (cardSlot3.GetComponentInChildren<Animator>() != null)
        {
            cardSlot3.GetComponentInChildren<Animator>().SetTrigger("Flip");
            yield return new WaitForSeconds(waitToFlip);
            reproduce.PlayOneShot(flipCard);
        }

        yield return new WaitForSeconds(timeToWait);

        // action
        showTurnAction.ActionInShowTurn(false, 3);

        // wait
        yield return new WaitForSeconds(1f);

        // show the third card IA
        if (cardSlot6.GetComponentInChildren<Animator>() != null)
        {
            cardSlot6.GetComponentInChildren<Animator>().SetTrigger("Flip");
            yield return new WaitForSeconds(waitToFlip);
            reproduce.PlayOneShot(flipCard);
        }

        yield return new WaitForSeconds(timeToWait);

        // action
        showTurnAction.ActionInShowTurn(true, 6);

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

        yield return new WaitForSeconds(timeToWait);
        // show the first card AI
        if (cardSlot4.GetComponentInChildren<Animator>() != null)
        {
            cardSlot4.GetComponentInChildren<Animator>().SetTrigger("Flip");
            yield return new WaitForSeconds(waitToFlip);
            reproduce.PlayOneShot(flipCard);
        }

        yield return new WaitForSeconds(timeToWait);

        // action
        showTurnAction.ActionInShowTurn(true, 4);

        // wait
        yield return new WaitForSeconds(1f);

        // show the first card
        if (cardSlot1.GetComponentInChildren<Animator>() != null)
        {
            cardSlot1.GetComponentInChildren<Animator>().SetTrigger("Flip");
            yield return new WaitForSeconds(waitToFlip);
            reproduce.PlayOneShot(flipCard);
        }

        yield return new WaitForSeconds(timeToWait);

        // action
        showTurnAction.ActionInShowTurn(false, 1);

        // wait
        yield return new WaitForSeconds(1f);

        // show the second card AI
        if (cardSlot5.GetComponentInChildren<Animator>() != null)
        {
            cardSlot5.GetComponentInChildren<Animator>().SetTrigger("Flip");
            yield return new WaitForSeconds(waitToFlip);
            reproduce.PlayOneShot(flipCard);
        }

        yield return new WaitForSeconds(timeToWait);

        // action
        showTurnAction.ActionInShowTurn(true, 5);

        // wait
        yield return new WaitForSeconds(1f);

        // show the second card
        if (cardSlot2.GetComponentInChildren<Animator>() != null)
        {
            cardSlot2.GetComponentInChildren<Animator>().SetTrigger("Flip");
            yield return new WaitForSeconds(waitToFlip);
            reproduce.PlayOneShot(flipCard);
        }

        yield return new WaitForSeconds(timeToWait);

        // action
        showTurnAction.ActionInShowTurn(false, 2);

        // wait
        yield return new WaitForSeconds(1f);

        // show the third card AI
        if (cardSlot6.GetComponentInChildren<Animator>() != null)
        {
            cardSlot6.GetComponentInChildren<Animator>().SetTrigger("Flip");
            yield return new WaitForSeconds(waitToFlip);
            reproduce.PlayOneShot(flipCard);
        }

        yield return new WaitForSeconds(timeToWait);

        // action
        showTurnAction.ActionInShowTurn(true, 6);

        // wait
        yield return new WaitForSeconds(1f);

        // show the third card
        if (cardSlot3.GetComponentInChildren<Animator>() != null)
        {
            cardSlot3.GetComponentInChildren<Animator>().SetTrigger("Flip");
            yield return new WaitForSeconds(waitToFlip);
            reproduce.PlayOneShot(flipCard);
        }

        yield return new WaitForSeconds(timeToWait);

        // action
        showTurnAction.ActionInShowTurn(false, 3);

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
        UIManager.instance.slot1card = 6;
        UIManager.instance.slot2card = 6;
        UIManager.instance.slot3card = 6;
        UIManager.instance.slot4card = 6;
        UIManager.instance.slot5card = 6;
        UIManager.instance.slot6card = 6;
    }
}
