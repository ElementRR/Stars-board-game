using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnaAI : EnemyAI
{
    protected override void Awake()
    {
        if (ScoreManager.instance._enemyName != Enemy.Name.Ana)
        {
            profilePhoto.SetActive(false);
            Destroy(GetComponent<AnaAI>());
            return;
        }

        profilePhoto.SetActive(true);
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

        int enemyStars = GameManager.enemyStars;

        while (cardsToPlay.Count < 3)
        {
            if (enemyStars < 3)
            {
                cardsToPlay.Add(7);
                enemyStars += 2;
                continue;
            }

            // check towers in the field and add inhibitors

            for (int i = 0; i < 2; i++)
            {
                int towerIndex = GetFieldCards(i);

                if ((towerIndex == 0 || towerIndex == 2) && !cardsToPlay.Contains(5))
                {
                    cardsToPlay.Add(5);
                    break;
                }else if((towerIndex == 1 || towerIndex == 3) && !cardsToPlay.Contains(4))
                {
                    cardsToPlay.Add(4);
                    break;
                }
                else if (!cardsToChooseFrom.Contains(6) && !cardsToPlay.Contains(6) && !firstTurn)
                {
                    cardsToPlay.Add(6);
                    continue;
                }
                else
                {break;}
            }

            if (cardsToPlay.Count > 2)
            {
                break;
            }

            int random = Random.Range(0, 9);

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
    protected int GetFieldCards(int fieldNumber)
    {
        return showTurnAction.fieldSlots[fieldNumber].GetComponent<FieldSlot>().towerToInstantiate;
    }
}
