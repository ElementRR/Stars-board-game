using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private ShowTurnAction showTurnAction;
    private EnemyAI enemyAI;

    public bool actionTurn;
    public bool showFase1 = true;

    public int cardCount;
    public GameObject cardFlipped;
    public GameObject cardSlot1;
    public GameObject cardSlot2;
    public GameObject cardSlot3;
    public GameObject cardSlot4;
    public GameObject cardSlot5;
    public GameObject cardSlot6;

    public GameObject[] FieldcardIndex;

    void Awake()
    {
        instance = this;
        cardCount = 0;
        actionTurn = true;
        showTurnAction = GetComponent<ShowTurnAction>();
        enemyAI = GetComponent<EnemyAI>();
    }

    public void InstantiateInSlot(GameObject cardSlot, bool isEnemy)
    {
        Quaternion newRotation = transform.rotation * Quaternion.Euler(0, 180, 0);
        Instantiate(cardFlipped, cardSlot.transform.position, newRotation, cardSlot.transform);

        if (!isEnemy)
        {
            cardCount++;
        }
    }

    public void FillSlot()
    {
        if (actionTurn)
        {
            if (actionTurn && cardCount < 3)
            {
                if (cardCount < 1)
                {
                    InstantiateInSlot(cardSlot1, false);
                }
                else if (cardCount < 2)
                {
                    InstantiateInSlot(cardSlot2, false);
                }
                else if (cardCount < 3)
                {
                    InstantiateInSlot(cardSlot3, false);
                }
            }
        }
    }

    public void EndTurn()
    {
        if (cardCount > 2)
        {
            var clones = GameObject.FindGameObjectsWithTag("FlippedCard");
            foreach (var clone in clones)
            {
                Destroy(clone);
            }

            cardCount = 0;
            actionTurn = false;
            ButtonAnimations.instance.EndActionT();
            if (showFase1)
            {
                StartCoroutine(ShowTurn1());
                showFase1 = false;
            }
            else
            {
                StartCoroutine(ShowTurn2());
                showFase1 = true;
            }
        }
    }


    private IEnumerator ShowTurn1()
    {
        static int ShowFase(int faseNumber)
        {
            if (faseNumber == 1)
            {
                return UIManager.instance.slot1card;
            } else if (faseNumber == 2)
            {
                return UIManager.instance.slot2card;
            }
            else if (faseNumber == 3)
            {
                return UIManager.instance.slot3card;
            }
            else if (faseNumber == 4)
            {
                return UIManager.instance.slot4card;
            }
            else if (faseNumber == 5)
            {
                return UIManager.instance.slot5card;
            }
            else if (faseNumber == 6)
            {
                return UIManager.instance.slot6card;
            }
            else
            {
                return 0;
            }
        }

        yield return new WaitForSeconds(1.5f);
        // show the first card
        Instantiate(FieldcardIndex[ShowFase(1)], cardSlot1.transform.position,
            cardSlot1.transform.rotation * Quaternion.Euler(0, 180, 0), cardSlot1.transform);

        yield return new WaitForSeconds(1f);

        // action
        showTurnAction.ActionInShowTurn(false, 1);

        // wait
        yield return new WaitForSeconds(1.5f);

        // show the first card AI
        Instantiate(FieldcardIndex[ShowFase(4)], cardSlot4.transform.position,
            cardSlot4.transform.rotation * Quaternion.Euler(0, 180, 0), cardSlot4.transform);

        yield return new WaitForSeconds(1f);

        // action
        showTurnAction.ActionInShowTurn(true, 4);

        // wait
        yield return new WaitForSeconds(1.5f);

        // show the second card
        Instantiate(FieldcardIndex[ShowFase(2)], cardSlot2.transform.position,
            cardSlot2.transform.rotation * Quaternion.Euler(0, 180, 0), cardSlot2.transform);

        yield return new WaitForSeconds(1f);

        // action
        showTurnAction.ActionInShowTurn(false, 2);

        // wait
        yield return new WaitForSeconds(1.5f);
        // show the second card AI
        Instantiate(FieldcardIndex[ShowFase(5)], cardSlot5.transform.position,
            cardSlot5.transform.rotation * Quaternion.Euler(0, 180, 0), cardSlot5.transform);

        yield return new WaitForSeconds(1f);

        // action
        showTurnAction.ActionInShowTurn(true, 5);

        // wait
        yield return new WaitForSeconds(1.5f);

        // show the third card
        Instantiate(FieldcardIndex[ShowFase(3)], cardSlot3.transform.position,
            cardSlot3.transform.rotation * Quaternion.Euler(0, 180, 0), cardSlot3.transform);

        yield return new WaitForSeconds(1f);

        // action
        showTurnAction.ActionInShowTurn(false, 3);

        // wait
        yield return new WaitForSeconds(1.5f);

        // show the third card IA
        Instantiate(FieldcardIndex[ShowFase(6)], cardSlot6.transform.position,
            cardSlot5.transform.rotation * Quaternion.Euler(0, 180, 0), cardSlot6.transform);

        yield return new WaitForSeconds(1f);

        // action
        showTurnAction.ActionInShowTurn(true, 6);

        // wait
        yield return new WaitForSeconds(1.5f);

        // reset slots
        ResetSlots();

        // reset action turn
        actionTurn = true;
        ButtonAnimations.instance.EndShowT();
        enemyAI.EnemyPlay();
    }

    private IEnumerator ShowTurn2()
    {
        static int ShowFase(int faseNumber)
        {
            if (faseNumber == 1)
            {
                return UIManager.instance.slot1card;
            }
            else if (faseNumber == 2)
            {
                return UIManager.instance.slot2card;
            }
            else if (faseNumber == 3)
            {
                return UIManager.instance.slot3card;
            }
            else if (faseNumber == 4)
            {
                return UIManager.instance.slot4card;
            }
            else if (faseNumber == 5)
            {
                return UIManager.instance.slot5card;
            }
            else if (faseNumber == 6)
            {
                return UIManager.instance.slot6card;
            }
            else
            {
                return 0;
            }
        }

        // wait
        yield return new WaitForSeconds(1.5f);

        // show the first card AI
        Instantiate(FieldcardIndex[ShowFase(4)], cardSlot4.transform.position,
            cardSlot4.transform.rotation * Quaternion.Euler(0, 180, 0), cardSlot4.transform);

        yield return new WaitForSeconds(1f);

        // action
        showTurnAction.ActionInShowTurn(true, 4);


        yield return new WaitForSeconds(1.5f);
        // show the first card
        Instantiate(FieldcardIndex[ShowFase(1)], cardSlot1.transform.position,
            cardSlot1.transform.rotation * Quaternion.Euler(0, 180, 0), cardSlot1.transform);

        yield return new WaitForSeconds(1f);

        // action
        showTurnAction.ActionInShowTurn(false, 1);
       
        // wait
        yield return new WaitForSeconds(1.5f);
        // show the second card AI
        Instantiate(FieldcardIndex[ShowFase(5)], cardSlot5.transform.position,
            cardSlot5.transform.rotation * Quaternion.Euler(0, 180, 0), cardSlot5.transform);

        yield return new WaitForSeconds(1f);

        // action
        showTurnAction.ActionInShowTurn(true, 5);

        // wait
        yield return new WaitForSeconds(1.5f);

        // show the second card
        Instantiate(FieldcardIndex[ShowFase(2)], cardSlot2.transform.position,
            cardSlot2.transform.rotation * Quaternion.Euler(0, 180, 0), cardSlot2.transform);

        yield return new WaitForSeconds(1f);

        // action
        showTurnAction.ActionInShowTurn(false, 2);

        // wait
        yield return new WaitForSeconds(1.5f);

        // show the third card IA
        Instantiate(FieldcardIndex[ShowFase(6)], cardSlot6.transform.position,
            cardSlot5.transform.rotation * Quaternion.Euler(0, 180, 0), cardSlot6.transform);

        yield return new WaitForSeconds(1f);

        // action
        showTurnAction.ActionInShowTurn(true, 6);

        // wait
        yield return new WaitForSeconds(1.5f);

        // show the third card
        Instantiate(FieldcardIndex[ShowFase(3)], cardSlot3.transform.position,
            cardSlot3.transform.rotation * Quaternion.Euler(0, 180, 0), cardSlot3.transform);

        yield return new WaitForSeconds(1f);

        // action
        showTurnAction.ActionInShowTurn(false, 3);

        // wait
        yield return new WaitForSeconds(1.5f);

        // reset slots
        ResetSlots();

        // reset action turn
        actionTurn = true;
        ButtonAnimations.instance.EndShowT();
        enemyAI.EnemyPlay();
    }

    public void ResetSlots()
    {
        UIManager.instance.slot1card = 6;
        UIManager.instance.slot2card = 6;
        UIManager.instance.slot3card = 6;
    }
}
