using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    protected ShowTurnAction showTurnAction;
    [SerializeField] protected List<int> cardsToChooseFrom;
    [SerializeField] protected List<int> cardsToPlay;

    protected bool firstTurn = true;

    protected void Start()
    {
        GameManager.instance.OnFirstTurnEnd += InsertCard6;
    }

    protected virtual void Awake()
    {
        showTurnAction = GetComponent<ShowTurnAction>();
        EnemyPlay();
        firstTurn = true;
    }
    public virtual void EnemyPlay()
    {
        cardsToChooseFrom = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 7 });

        if (!firstTurn)
        {
            cardsToChooseFrom.Add(6);
        }

        for (int i = 3; i < 6; i++)
        {
            cardsToChooseFrom.Remove(GetFieldCards(i));
        }

        cardsToPlay = new();

        int enemyStars = GameManager.enemyStars;

        while (cardsToPlay.Count < 3)
        {
            if (enemyStars < 3)
            {
                cardsToPlay.Add(7);
                enemyStars += 2;
                continue;
            }

            int random = Random.Range(0, 9);

            if(random > 7)
            {
                random = 7;
            }

            foreach (int item in cardsToChooseFrom)
            {
                if(random == 7)
                {
                    cardsToPlay.Add(random);
                    break;
                }

                if (item == random && !cardsToPlay.Contains(random))
                {
                    cardsToPlay.Add(random);
                    break;
                }
            }
        }

        for (int i = 3; i < 6; i++)
        {
            GameManager.instance.InstantiateInSlot(GameManager.instance.cardSlots[i], cardsToPlay[i - 3]);
        }
    }
    protected int GetFieldCards(int fieldNumber)
    {
        return showTurnAction.fieldSlots[fieldNumber].GetComponent<FieldSlot>().towerToInstantiate;
    }

    protected void InsertCard6()
    {
        firstTurn = false;
    }
}
