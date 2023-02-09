using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private ShowTurnAction showTurnAction;
    [SerializeField]
    int[] fieldTowers;
    private void Start()
    {
        showTurnAction = GetComponent<ShowTurnAction>();
        EnemyPlay();
    }
    public void EnemyPlay()
    {
        if (GameManager.instance.actionTurn)
        {
            fieldTowers = new int[] { GetFieldCards(3), GetFieldCards(4), GetFieldCards(5) };

            foreach (var item in fieldTowers)
            {
                while (UIManager.instance.slot4card == item)
                {
                    UIManager.instance.slot4card = Random.Range(0, 6);
                }

                while (UIManager.instance.slot4card == UIManager.instance.slot5card || UIManager.instance.slot5card == item)
                {
                    UIManager.instance.slot5card = Random.Range(0, 6);
                }

                while (UIManager.instance.slot6card == UIManager.instance.slot4card ||
                    UIManager.instance.slot6card == UIManager.instance.slot5card ||
                    UIManager.instance.slot6card == item)
                {
                    UIManager.instance.slot6card = Random.Range(0, 6);
                }
            }


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
