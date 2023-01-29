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

    void Awake()
    {
        instance = this;
        cardCount = 0;
        actionTurn = true;
    }

    private void InstantiateInSlot(GameObject cardSlot)
    {
        Instantiate(cardFlipped, cardSlot.transform.position, transform.rotation);
        cardCount++;
    }


    public void FillSlot()
    {
        if(actionTurn && cardCount < 3)
        {
            if(cardCount < 1)
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
    if(cardCount > 2)
        {
            var clones = GameObject.FindGameObjectsWithTag("FlippedCard");
            foreach (var clone in clones)
            {
                Destroy(clone);
            }

            cardCount = 0;
            actionTurn = false;
        }
    }

    private void Update()
    {
        if (!actionTurn)
        {
            // StartCoroutine();
        }
    }

    private IEnumerator ShowTurn()
    {
        yield return new WaitForSeconds(1.5f);
        // vira primeira carta
        // ação
        // vira segunda carta
        // ação
        // vira terceira carta
        // ação
        // retornar cartas para a mão
        // 
    }
}
