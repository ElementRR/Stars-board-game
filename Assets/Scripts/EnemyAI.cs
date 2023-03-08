using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private ShowTurnAction showTurnAction;
    [SerializeField] List<int> cardsToChooseFrom;
    [SerializeField] List<int> cardsToPlay;
    private void Awake()
    {
        showTurnAction = GetComponent<ShowTurnAction>();
        EnemyPlay();
    }
    public void EnemyPlay()
    {
        cardsToChooseFrom = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6 });
        for (int i = 3; i < 6; i++)
        {
            cardsToChooseFrom.Remove(GetFieldCards(i));
        }

        cardsToPlay = new();

        while (cardsToPlay.Count < 3)
        {
            int random = Random.Range(0, 6);
            foreach (int item in cardsToChooseFrom)
            {
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

        //UIManager.instance.slot4card = cardsToChooseFrom[Random.Range(0, cardsToChooseFrom.Count)];
        //cardsToChooseFrom.Remove(UIManager.instance.slot4card);

        //UIManager.instance.slot5card = cardsToChooseFrom[Random.Range(0, cardsToChooseFrom.Count)];
        //cardsToChooseFrom.Remove(UIManager.instance.slot5card);

        //UIManager.instance.slot6card = cardsToChooseFrom[Random.Range(0, cardsToChooseFrom.Count)];


        //if (GameManager.instance.actionTurn)
        //{
        //    GameManager.instance.InstantiateInSlot(GameManager.instance.cardSlot4, UIManager.instance.slot4card);
        //    GameManager.instance.InstantiateInSlot(GameManager.instance.cardSlot5, UIManager.instance.slot5card);
         //   GameManager.instance.InstantiateInSlot(GameManager.instance.cardSlot6, UIManager.instance.slot6card);
        //}
    }
    private int GetFieldCards(int fieldNumber)
    {
        return showTurnAction.fieldSlots[fieldNumber].GetComponent<FieldSlot>().towerToInstantiate;
    }
}
