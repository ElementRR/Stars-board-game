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

    public GameObject[] FieldcardIndex;

    void Awake()
    {
        instance = this;
        cardCount = 0;
        actionTurn = true;
        showTurnAction = GetComponent<ShowTurnAction>();
    }

    private void InstantiateInSlot(GameObject cardSlot)
    {
        Quaternion newRotation = transform.rotation * Quaternion.Euler(0, 180, 0);
        Instantiate(cardFlipped, cardSlot.transform.position, newRotation);
        cardCount++;
    }

    public void FillSlot()
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
        int ShowFase(int fase, int subfase)
        {
            if (fase == 1 && subfase == 1)
            {
                return UIManager.instance.slot1card;
            } else if (fase == 2 && subfase == 1)
            {
                return UIManager.instance.slot2card;
            }
            else if (fase == 3 && subfase == 1)
            {
                return UIManager.instance.slot3card;
            }
            else
            {
                return 0;
            }
        }

        yield return new WaitForSeconds(2f);
        // show the first card
        Instantiate(FieldcardIndex[ShowFase(1, 1)], cardSlot1.transform.position,
            cardSlot1.transform.rotation * Quaternion.Euler(0, 180, 0));
        // action

        showTurnAction.ActionInShowTurn(false, 1);

        // show the first card AI
        // action
        // wait
        yield return new WaitForSeconds(2f);
        // show the second card
        Instantiate(FieldcardIndex[ShowFase(2, 1)], cardSlot2.transform.position,
            cardSlot2.transform.rotation * Quaternion.Euler(0, 180, 0));
        // action
        // show the second card AI
        // action
        // wait
        yield return new WaitForSeconds(2f);
        // show the third card
        Instantiate(FieldcardIndex[ShowFase(3, 1)], cardSlot3.transform.position,
            cardSlot3.transform.rotation * Quaternion.Euler(0, 180, 0));
        // action
        // show the third card IA
        // action
        // wait
        yield return new WaitForSeconds(2f);
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
