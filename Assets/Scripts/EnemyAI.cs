using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // sometimes the AI is intalling 2 of the same towers

    private ShowTurnAction showTurnAction;
    [SerializeField] List<int> cardsToChooseFrom;
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

        UIManager.instance.slot4card = cardsToChooseFrom[Random.Range(0, cardsToChooseFrom.Count)];
        cardsToChooseFrom.Remove(UIManager.instance.slot4card);

        UIManager.instance.slot5card = cardsToChooseFrom[Random.Range(0, cardsToChooseFrom.Count)];
        cardsToChooseFrom.Remove(UIManager.instance.slot5card);

        UIManager.instance.slot6card = cardsToChooseFrom[Random.Range(0, cardsToChooseFrom.Count)];


        if (GameManager.instance.actionTurn)
        {
            GameManager.instance.InstantiateInSlot(GameManager.instance.cardSlot4, true, UIManager.instance.slot4card);
            GameManager.instance.InstantiateInSlot(GameManager.instance.cardSlot5, true, UIManager.instance.slot5card);
            GameManager.instance.InstantiateInSlot(GameManager.instance.cardSlot6, true, UIManager.instance.slot6card);
        }
    }
    private int GetFieldCards(int fieldNumber)
    {
        return showTurnAction.fieldSlots[fieldNumber].GetComponent<FieldSlot>().towerToInstantiate;
    }
}
