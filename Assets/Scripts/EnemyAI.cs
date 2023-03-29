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
        cardsToChooseFrom = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7 });
        for (int i = 3; i < 6; i++)
        {
            cardsToChooseFrom.Remove(GetFieldCards(i));
        }

        cardsToPlay = new();

        while (cardsToPlay.Count < 3)
        {
            int random = Random.Range(0, 7);
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
    }
    private int GetFieldCards(int fieldNumber)
    {
        return showTurnAction.fieldSlots[fieldNumber].GetComponent<FieldSlot>().towerToInstantiate;
    }
}
