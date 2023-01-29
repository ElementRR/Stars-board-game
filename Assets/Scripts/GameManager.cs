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

    void Start()
    {
        instance = this;
        cardCount = 0;
        actionTurn = true;
    }

    public void FillSlot()
    {
        if(actionTurn && cardCount < 3)
        {
            if(cardCount < 1)
            {
                Instantiate(cardFlipped, cardSlot1.transform.position, transform.rotation);
                cardCount++;
            }
            else if (cardCount < 2)
            {
                Instantiate(cardFlipped, cardSlot2.transform.position, transform.rotation);
                cardCount++;
            }
            else if (cardCount < 3)
            {
                Instantiate(cardFlipped, cardSlot3.transform.position, transform.rotation);
                cardCount++;
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
}
