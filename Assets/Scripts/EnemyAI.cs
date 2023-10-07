using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private ShowTurnAction showTurnAction;
    [SerializeField] List<int> cardsToChooseFrom;
    [SerializeField] List<int> cardsToPlay;

    private bool firstTurn = true;

    private void Start()
    {
        GameManager.instance.OnFirstTurnEnd += InsertCard6;
    }

    private void Awake()
    {
        showTurnAction = GetComponent<ShowTurnAction>();
        EnemyPlay();
        firstTurn = true;
    }
    public void EnemyPlay()
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

        int enemyStars = GameManager.instance.enemyStars;

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
    private int GetFieldCards(int fieldNumber)
    {
        return showTurnAction.fieldSlots[fieldNumber].GetComponent<FieldSlot>().towerToInstantiate;
    }

    private void InsertCard6()
    {
        firstTurn = false;
    }
}
