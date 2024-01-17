using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdAI : EnemyAI
{
    // Insert specific logic for Ed
    protected override void Awake()
    {

        if (ScoreManager.instance._enemyName != Enemy.Name.Ed)
        {
            profilePhoto.SetActive(false);
            Destroy(GetComponent<EdAI>());
            return;
        }

        showTurnAction = GetComponent<ShowTurnAction>();
        firstTurn = true;
        EnemyPlay();
    }
    public override void EnemyPlay()
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

        //int enemyStars = GameManager.enemyStars;

        while (cardsToPlay.Count < 3)
        {
            //if (enemyStars < 3)
            //{
             //   cardsToPlay.Add(7);
            //    enemyStars += 2;
            //    continue;
            //}

            int random = Random.Range(0, 11);

            if (random > 7)
            {
                random = 7;
            }

            foreach (int item in cardsToChooseFrom)
            {
                if (random == 7)
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
}
