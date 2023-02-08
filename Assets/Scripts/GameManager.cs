using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShowTurnAction))]
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

    private Animator cardAnimator;

    public GameObject[] FieldcardIndex;

    void Awake()
    {
        instance = this;
        cardCount = 0;
        actionTurn = true;
        showFase1 = true;
        showTurnAction = GetComponent<ShowTurnAction>();
        enemyAI = GetComponent<EnemyAI>();
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
            //var clones = GameObject.FindGameObjectsWithTag("FlippedCard");
            //foreach (var clone in clones)
            //{
             //   Destroy(clone);
            //}

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

        yield return new WaitForSeconds(1.5f);
        // show the first card
        //Instantiate(FieldcardIndex[ShowFase(1)], cardSlot1.transform.position,
        //cardSlot1.transform.rotation * Quaternion.Euler(0, 180, 0), cardSlot1.transform);
        cardSlot1.GetComponentInChildren<Animator>().SetTrigger("Flip");


        yield return new WaitForSeconds(timeToWait);

        // action
        showTurnAction.ActionInShowTurn(false, 1);

        // wait
        yield return new WaitForSeconds(1f);

        // show the first card AI
        cardSlot4.GetComponentInChildren<Animator>().SetTrigger("Flip");

        yield return new WaitForSeconds(timeToWait);

        // action
        showTurnAction.ActionInShowTurn(true, 4);

        // wait
        yield return new WaitForSeconds(1f);

        // show the second card
        cardSlot2.GetComponentInChildren<Animator>().SetTrigger("Flip");

        yield return new WaitForSeconds(timeToWait);

        // action
        showTurnAction.ActionInShowTurn(false, 2);

        // wait
        yield return new WaitForSeconds(1f);
        // show the second card AI
        cardSlot5.GetComponentInChildren<Animator>().SetTrigger("Flip");

        yield return new WaitForSeconds(timeToWait);

        // action
        showTurnAction.ActionInShowTurn(true, 5);

        // wait
        yield return new WaitForSeconds(1f);

        // show the third cards
        cardSlot3.GetComponentInChildren<Animator>().SetTrigger("Flip");

        yield return new WaitForSeconds(timeToWait);

        // action
        showTurnAction.ActionInShowTurn(false, 3);

        // wait
        yield return new WaitForSeconds(1f);

        // show the third card IA
        cardSlot6.GetComponentInChildren<Animator>().SetTrigger("Flip");

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

        yield return new WaitForSeconds(timeToWait);
        // show the first card AI
        cardSlot4.GetComponentInChildren<Animator>().SetTrigger("Flip");

        yield return new WaitForSeconds(timeToWait);

        // action
        showTurnAction.ActionInShowTurn(true, 4);

        // wait
        yield return new WaitForSeconds(1f);

        // show the first card
        cardSlot1.GetComponentInChildren<Animator>().SetTrigger("Flip");

        yield return new WaitForSeconds(timeToWait);

        // action
        showTurnAction.ActionInShowTurn(false, 1);

        // wait
        yield return new WaitForSeconds(1f);

        // show the second card AI
        cardSlot5.GetComponentInChildren<Animator>().SetTrigger("Flip");

        yield return new WaitForSeconds(timeToWait);

        // action
        showTurnAction.ActionInShowTurn(true, 5);

        // wait
        yield return new WaitForSeconds(1f);

        // show the second card
        cardSlot2.GetComponentInChildren<Animator>().SetTrigger("Flip");

        yield return new WaitForSeconds(timeToWait);

        // action
        showTurnAction.ActionInShowTurn(false, 2);

        // wait
        yield return new WaitForSeconds(1f);

        // show the third card AI
        cardSlot6.GetComponentInChildren<Animator>().SetTrigger("Flip");

        yield return new WaitForSeconds(timeToWait);

        // action
        showTurnAction.ActionInShowTurn(true, 6);

        // wait
        yield return new WaitForSeconds(1f);

        // show the third card
        cardSlot3.GetComponentInChildren<Animator>().SetTrigger("Flip");

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
