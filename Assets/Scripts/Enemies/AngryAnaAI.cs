using System.Collections.Generic;
using UnityEngine;

public class AngryAnaAI : EnemyAI
{
    public delegate void Outdoor(string message);
    public static event Outdoor OnMessageSent;

    protected override void Awake()
    {
        if (ScoreManager.instance._enemyName != Enemy.Name.AngryAna)
        {
            profilePhoto.SetActive(false);
            Destroy(GetComponent<AngryAnaAI>());
            return;
        }

        profilePhoto.SetActive(true);
        showTurnAction = GetComponent<ShowTurnAction>();
        firstTurn = true;
        Settings.enemyTowerSkins = new(new int[] { 1, 0, 0, 1, 0 });
        EnemyPlay();
    }

    public override void EnemyPlay()
    {
        GameManager.enemyStars += 1;
        UIManager.instance.enemyStarCount.text = "" + GameManager.enemyStars;
        OnMessageSent?.Invoke("Ana received 2 stars!");

        cardsToChooseFrom = new List<int>(new int[] { 0, 1, 2, 3, 7 });

        if (!firstTurn)
        {
            cardsToChooseFrom.Add(4);
            cardsToChooseFrom.Add(5);
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
            // if have less than 3 stars, use starcard

            if (enemyStars < 3)
            {
                cardsToPlay.Add(7);
                enemyStars += 2;
                continue;
            }

            //if Ana have 2 towers installed and more than 2 stars, choose a tower

            bool isFieldSlot3filled = showTurnAction.fieldSlots[3].GetComponent<FieldSlot>().isFilled;
            bool isFieldSlot4filled = showTurnAction.fieldSlots[4].GetComponent<FieldSlot>().isFilled;

            if (isFieldSlot3filled && isFieldSlot4filled && !GameManager.instance.showFase1 && enemyStars > 2)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (cardsToChooseFrom.Contains(i))
                    {
                        cardsToPlay.Add(i);
                        cardsToChooseFrom.Remove(i);
                        break;
                    }
                }
            }

            // check towers in the field and add inhibitors

            for (int i = 0; i < 2; i++)
            {
                int towerIndex = GetFieldCards(i);

                if ((towerIndex == 0 || towerIndex == 2) && !cardsToPlay.Contains(5))
                {
                    cardsToPlay.Add(5);
                    continue;
                }
                else if ((towerIndex == 1 || towerIndex == 3) && !cardsToPlay.Contains(4))
                {
                    cardsToPlay.Add(4);
                    continue;
                }
                else if (towerIndex == 6)
                {
                    cardsToChooseFrom.Remove(6);
                    continue;
                }
                else if (cardsToChooseFrom.Contains(6) && !cardsToPlay.Contains(6) && !firstTurn)
                {
                    cardsToPlay.Add(6);
                    continue;
                }
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
