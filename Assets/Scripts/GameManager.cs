using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private ShowTurnAction showTurnAction;

    public bool actionTurn;
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
    }

    public void InstantiateInSlot(GameObject cardSlot)
    {
        Quaternion newRotation = transform.rotation * Quaternion.Euler(0, 180, 0);
        Instantiate(cardFlipped, cardSlot.transform.position, newRotation, cardSlot.transform);
        cardCount++;
    }

    public void FillSlot()
    {
        if (actionTurn)
        {
            if (actionTurn && cardCount < 3)
            {
                if (cardCount < 1)
                {
                    InstantiateInSlot(cardSlot1);
                }
                else if (cardCount < 2)
                {
                    InstantiateInSlot(cardSlot2);
                }
                else if (cardCount < 3)
                {
                    InstantiateInSlot(cardSlot3);
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
            StartCoroutine(ShowTurn());
        }
    }


    private IEnumerator ShowTurn()
    {
        int ShowFase(int faseNumber)
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
        // action
        showTurnAction.ActionInShowTurn(false, 1);

        // wait
        yield return new WaitForSeconds(1.5f);

        // show the first card AI
        Instantiate(FieldcardIndex[ShowFase(4)], cardSlot4.transform.position,
            cardSlot4.transform.rotation * Quaternion.Euler(0, 180, 0), cardSlot4.transform);

        // action
        showTurnAction.ActionInShowTurn(true, 4);

        // wait
        yield return new WaitForSeconds(1.5f);

        // show the second card
        Instantiate(FieldcardIndex[ShowFase(2)], cardSlot2.transform.position,
            cardSlot2.transform.rotation * Quaternion.Euler(0, 180, 0), cardSlot2.transform);

        // action
        showTurnAction.ActionInShowTurn(false, 2);

        // wait
        yield return new WaitForSeconds(1.5f);
        // show the second card AI
        Instantiate(FieldcardIndex[ShowFase(5)], cardSlot5.transform.position,
            cardSlot5.transform.rotation * Quaternion.Euler(0, 180, 0), cardSlot5.transform);

        // action
        showTurnAction.ActionInShowTurn(true, 5);

        // wait
        yield return new WaitForSeconds(1.5f);

        // show the third card
        Instantiate(FieldcardIndex[ShowFase(2)], cardSlot3.transform.position,
            cardSlot3.transform.rotation * Quaternion.Euler(0, 180, 0), cardSlot3.transform);

        // action
        showTurnAction.ActionInShowTurn(false, 3);

        // wait
        yield return new WaitForSeconds(1.5f);

        // show the third card IA
        Instantiate(FieldcardIndex[ShowFase(6)], cardSlot5.transform.position,
            cardSlot5.transform.rotation * Quaternion.Euler(0, 180, 0), cardSlot5.transform);

        // action
        showTurnAction.ActionInShowTurn(true, 6);

        // wait
        yield return new WaitForSeconds(1.5f);

        // return cards to hand
        // reset slots
        ResetSlots();
        // return non-used cards
        // reset action turn
        actionTurn = true;
        ButtonAnimations.instance.EndShowT();
    }

    public void ResetSlots()
    {
        UIManager.instance.slot1card = 6;
        UIManager.instance.slot2card = 6;
        UIManager.instance.slot3card = 6;
    }
}
