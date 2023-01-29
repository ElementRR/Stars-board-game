using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
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

    private void Update()
    {

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
        // vira primeira carta
        Instantiate(FieldcardIndex[ShowFase(1, 1)], cardSlot1.transform.position,
            cardSlot1.transform.rotation * Quaternion.Euler(0, 180, 0));
        // ação
        // vira primeira carta IA
        // ação
        // espera
        yield return new WaitForSeconds(2f);
        // vira segunda carta
        Instantiate(FieldcardIndex[ShowFase(2, 1)], cardSlot2.transform.position,
            cardSlot2.transform.rotation * Quaternion.Euler(0, 180, 0));
        // ação
        // vira segunda carta IA
        // ação
        // espera
        yield return new WaitForSeconds(2f);
        // vira terceira carta
        Instantiate(FieldcardIndex[ShowFase(3, 1)], cardSlot3.transform.position,
            cardSlot3.transform.rotation * Quaternion.Euler(0, 180, 0));
        // ação
        // vira terceira carta IA
        // ação
        // espera
        yield return new WaitForSeconds(2f);
        // retornar cartas para a mão
        // Zerar slots
        ResetSlots();
        // voltar cartas não usadas
        // recomeçar turno
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
