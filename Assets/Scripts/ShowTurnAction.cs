using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTurnAction : MonoBehaviour
{
    public static ShowTurnAction instance;
    public GameObject[] fieldSlots;

    public Tower[] enemy_towers;
    public Inhibitor enemy_inhibitor;
    public Tower[] me_towers;
    public Inhibitor me_inhibitor;

    public int me_value1 = 6;   // the index of the card in this fase
    public int me_value2 = 6;   // useless for now
    public int en_value1 = 6;   // the index of the enemy tower 1
    public int en_value2 = 6;   // the index of the enemy tower 2


    public bool enemyHasAnt;

    public int me_fCount;
    public int enemy_fCount;

    private void Awake()
    {
        instance = this;
    }
    public void ActionInShowTurn(bool isEnemy, int fase)
    {

        // me or enemy?
        if (!isEnemy)
        {
            // what showfase?
            if (fase == 1)
            {
                // tower or inhibitor?
                if (UIManager.instance.slot1card < 4)
                {
                    me_value1 = UIManager.instance.slot1card;
                    // me : tower: enemy have antagonist?
                    if (enemyHasAnt)
                    {
                        CheckEnemySlots();

                        CheckReturnCard(GameManager.instance.cardSlot1);
                    }
                    else
                    {
                        InstallTower(0, GameManager.instance.cardSlot1);
                    }
                }
            }
        }
        else
        {

        }

        // me : inhibitor: inhibit one tower from enemy
        // enemy : tower: me have antagonist?
        // yes: dont put
        // no: put
        // enemy : ihnibitor: inhibit one tower from me
    }

    private void CheckEnemySlots()
    {
        if (fieldSlots[3].GetComponent<FieldSlot>().isFilled)
        {
            en_value1 = fieldSlots[3].GetComponent<FieldSlot>().towerToInstantiate;
        }
        if (fieldSlots[4].GetComponent<FieldSlot>().isFilled)
        {
            en_value2 = fieldSlots[4].GetComponent<FieldSlot>().towerToInstantiate;
        }
    }
    private void CheckReturnCard(GameObject cardSlot)
    {
        if ((me_value1 == 0 || me_value2 == 0) && (en_value1 == 1 || en_value2 == 1))
        {
            UIManager.instance.ReturnCard(0, cardSlot);
        }
        else if ((me_value1 == 1 || me_value2 == 1) && (en_value1 == 0 || en_value2 == 0))
        {
            UIManager.instance.ReturnCard(1, cardSlot);
        }
        else if ((me_value1 == 2 || me_value2 == 2) && (en_value1 == 3 || en_value2 == 3))
        {
            UIManager.instance.ReturnCard(2, cardSlot);
        }
        else if ((me_value1 == 3 || me_value2 == 3) && (en_value1 == 2 || en_value2 == 2))
        {
            UIManager.instance.ReturnCard(3, cardSlot);
        }
        else
        {
            InstallTower(0, cardSlot);
        }
    }
    private void InstallTower(int slot, GameObject cardSlot)
    {
        fieldSlots[slot].GetComponent<FieldSlot>().towerToInstantiate = me_value1;
        fieldSlots[slot].GetComponent<FieldSlot>().InstantiateInSlot();
        Destroy(cardSlot.transform.GetChild(0).gameObject);
    }
}
