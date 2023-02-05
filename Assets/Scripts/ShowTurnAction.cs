using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// adversary = the opponent of the player we are looking
// enemy = the AI player
// me = the human player
public class ShowTurnAction : MonoBehaviour
{
    public GameObject[] fieldSlots;

    public Tower[] enemy_towers;
    public Inhibitor enemy_inhibitor;
    public Tower[] me_towers;
    public Inhibitor me_inhibitor;

    public int me_value1 = 6;   // the index of the card in this fase
    public int me_value2 = 6;   // useless for now
    public int en_value1 = 6;   // the index of the adversary tower 1
    public int en_value2 = 6;   // the index of the adversary tower 2

    int whereInstallT;

    private void Update()
    {
        if (fieldSlots[2].GetComponent<FieldSlot>().isFilled)
        {
            Debug.Log("Player wins!");
            Time.timeScale = 0;
        }
        if (fieldSlots[5].GetComponent<FieldSlot>().isFilled)
        {
            Debug.Log("AI wins!");
            Time.timeScale = 0;
        }
    }

    public void ActionInShowTurn(bool isEnemy, int faseNumber)
    {
        bool adversaryHasAnt;
        // me or enemy?
        if (isEnemy)
        {
            whereInstallT = 3;
        }
        else
        {
            whereInstallT = 0;
        }

        int slotcard;
        GameObject cardSlot1;
        // what fase?
        if (faseNumber == 1)
        {
            slotcard = UIManager.instance.slot1card;
            cardSlot1 = GameManager.instance.cardSlot1;
        }
        else if (faseNumber == 2)
        {
            slotcard = UIManager.instance.slot2card;
            cardSlot1 = GameManager.instance.cardSlot2;
        }
        else if (faseNumber == 3)
        {
            slotcard = UIManager.instance.slot3card;
            cardSlot1 = GameManager.instance.cardSlot3;
        }
        else if (faseNumber == 4)
        {
            slotcard = UIManager.instance.slot4card;
            cardSlot1 = GameManager.instance.cardSlot4;
        }
        else if (faseNumber == 5)
        {
            slotcard = UIManager.instance.slot5card;
            cardSlot1 = GameManager.instance.cardSlot5;
        }
        else if (faseNumber == 6)
        {
            slotcard = UIManager.instance.slot6card;
            cardSlot1 = GameManager.instance.cardSlot6;
        }
        else
        {
            slotcard = 7;
            cardSlot1 = GameManager.instance.cardSlot1;
        } // Fases 1.2 = 4, 2.2 = 5, 3.2 = 6

        int adversarySlot1;
        int adversarySlot2;
        if (!isEnemy)
        {
            adversarySlot1 = 3;
            adversarySlot2 = 4;
        }
        else
        {
            adversarySlot1 = 0;
            adversarySlot2 = 1;
        }

        if (fieldSlots[adversarySlot1].GetComponent<FieldSlot>().isFilled ||
            fieldSlots[adversarySlot2].GetComponent<FieldSlot>().isFilled)
        {
            adversaryHasAnt = true;
        }
        else
        {
            adversaryHasAnt = false;
        }


        me_value1 = slotcard;
        // tower or inhibitor?
        if (me_value1 < 4)
        {
            //  tower: adversary has antagonist?
            if (adversaryHasAnt) // yes : antagonist interacts with tower?
            {
                CheckAdversarySlots(isEnemy);

                CheckReturnCard(cardSlot1, isEnemy); // yes: return card
            }
            else // no : install tower
            {
                InstallTower(whereInstallT, cardSlot1);
            }
        }
        else //  inhibitor: adversary has tower?
        {
            if (adversaryHasAnt) // yes : tower interacts with inhibitor?
            {
                CheckAdversarySlots(isEnemy);

                DestroyAdversaryTower(isEnemy); // yes: destroy 1 enemy tower  

                UIManager.instance.ReturnCard(me_value1, cardSlot1, isEnemy); //return inhibitor card
            }
            else // no : return inhibitor card
            {
                UIManager.instance.ReturnCard(me_value1, cardSlot1, isEnemy);
            }
        }

        // enemy : tower: me have antagonist?
        // yes: dont put
        // no: put
        // enemy : ihnibitor: inhibit one tower from me
    }


    private void CheckAdversarySlots(bool isEnemy) // check if the adversary slots are filled and what tower is
        {
            int adversarySlot1;
            int adversarySlot2;
            if (!isEnemy)
            {
                adversarySlot1 = 3;
                adversarySlot2 = 4;
            }
            else
            {
                adversarySlot1 = 0;
                adversarySlot2 = 1;
            }

            if (fieldSlots[adversarySlot1].GetComponent<FieldSlot>().isFilled)
            {
                en_value1 = fieldSlots[adversarySlot1].GetComponent<FieldSlot>().towerToInstantiate;
            }
            if (fieldSlots[adversarySlot2].GetComponent<FieldSlot>().isFilled)
            {
                en_value2 = fieldSlots[adversarySlot2].GetComponent<FieldSlot>().towerToInstantiate;
            }
    }
    private void CheckReturnCard(GameObject cardSlot, bool isEnemy)
        {
            if ((me_value1 == 0 || me_value2 == 0) && (en_value1 == 1 || en_value2 == 1))
            {
                UIManager.instance.ReturnCard(0, cardSlot, isEnemy);
            }
            else if ((me_value1 == 1 || me_value2 == 1) && (en_value1 == 0 || en_value2 == 0))
            {
                UIManager.instance.ReturnCard(1, cardSlot, isEnemy);
            }
            else if ((me_value1 == 2 || me_value2 == 2) && (en_value1 == 3 || en_value2 == 3))
            {
                UIManager.instance.ReturnCard(2, cardSlot, isEnemy);
            }
            else if ((me_value1 == 3 || me_value2 == 3) && (en_value1 == 2 || en_value2 == 2))
            {
                UIManager.instance.ReturnCard(3, cardSlot, isEnemy);
            }
            else
            {
                InstallTower(whereInstallT, cardSlot);
            }
    }
    private void InstallTower(int slot, GameObject cardSlot)
        {
            if (!fieldSlots[slot].GetComponent<FieldSlot>().isFilled)
            {
                fieldSlots[slot].GetComponent<FieldSlot>().towerToInstantiate = me_value1;
                fieldSlots[slot].GetComponent<FieldSlot>().InstantiateInSlot();
            }
            else if(!fieldSlots[slot + 1].GetComponent<FieldSlot>().isFilled)
            {
                fieldSlots[slot + 1].GetComponent<FieldSlot>().towerToInstantiate = me_value1;
                fieldSlots[slot + 1].GetComponent<FieldSlot>().InstantiateInSlot();
            }
            else
            {
                fieldSlots[slot + 2].GetComponent<FieldSlot>().towerToInstantiate = me_value1;
                fieldSlots[slot + 2].GetComponent<FieldSlot>().InstantiateInSlot();
            }

            Destroy(cardSlot.transform.GetChild(0).gameObject);
            me_value1 = 6;
            me_value2 = 6;
            en_value1 = 6;
            en_value2 = 6;
    }
    private void DestroyAdversaryTower(bool isEnemy)
        {
            if (!isEnemy)
            {
                if (me_value1 == 4 && ((en_value1 == 1 || en_value2 == 3) || (en_value1 == 3 || en_value2 == 1)))
                {
                // Destroy 1 enemy cold tower
                Destroy1InhTower(3, 1);
                // return enemy card
                if (en_value2 == 1 || en_value2 == 3)
                    {
                    UIManager.instance.ReturnCard(en_value2, GameManager.instance.cardSlot5, !isEnemy);
                    }
                    else
                    {
                    UIManager.instance.ReturnCard(en_value1, GameManager.instance.cardSlot4, !isEnemy);
                    }
                

                } else if (me_value1 == 5 && ((en_value1 == 0 || en_value2 == 2) || (en_value1 == 2 || en_value2 == 0)))
                {
                // Destroy 1 enemy hot tower
                Destroy1InhTower(3, 0);

                    // and return enemy card
                    if (en_value2 == 0 || en_value2 == 2)
                    {
                    UIManager.instance.ReturnCard(en_value2, GameManager.instance.cardSlot5, !isEnemy);
                    }
                    else
                    {
                    UIManager.instance.ReturnCard(en_value1, GameManager.instance.cardSlot4, !isEnemy);
                    }
                }
            }
            else
            {
                if (me_value1 == 4 && ((en_value1 == 1 || en_value2 == 3) || (en_value1 == 3 || en_value2 == 1)))
                {
                // Destroy 1 me cold tower
                Destroy1InhTower(0, 1);

                // and return me card
                if (en_value2 == 1 || en_value2 == 3)
                {
                    UIManager.instance.ReturnCard(en_value2, GameManager.instance.cardSlot2, !isEnemy);
                }
                else
                {
                    UIManager.instance.ReturnCard(en_value1, GameManager.instance.cardSlot1, !isEnemy);
                }
            }
                else if (me_value1 == 5 && ((en_value1 == 0 || en_value2 == 2) || (en_value1 == 2 || en_value2 == 0)))
                {
                // Destroy 1 me hot tower
                Destroy1InhTower(0, 0);

                // and return me card
                if (en_value2 == 0 || en_value2 == 2)
                {
                    UIManager.instance.ReturnCard(en_value2, GameManager.instance.cardSlot2, !isEnemy);
                }
                else
                {
                    UIManager.instance.ReturnCard(en_value1, GameManager.instance.cardSlot1, !isEnemy);
                }
            }
            }


        }

    private void Destroy1InhTower(int minSlot, int hotNcoldValue)
    {
        if (fieldSlots[minSlot + 1].GetComponent<FieldSlot>().isFilled &&
                    (fieldSlots[minSlot + 1].GetComponent<FieldSlot>().towerToInstantiate == hotNcoldValue ||
                    fieldSlots[minSlot + 1].GetComponent<FieldSlot>().towerToInstantiate == hotNcoldValue + 2))
        {
            fieldSlots[minSlot + 1].GetComponent<FieldSlot>().towerToInstantiate = 6;
            fieldSlots[minSlot + 1].GetComponent<FieldSlot>().isFilled = false;
            Destroy(fieldSlots[minSlot + 1].transform.GetChild(0).gameObject);
        }
        else
        {
            fieldSlots[minSlot].GetComponent<FieldSlot>().towerToInstantiate = 6;
            fieldSlots[minSlot].GetComponent<FieldSlot>().isFilled = false;
            Destroy(fieldSlots[minSlot].transform.GetChild(0).gameObject);
        }
    }
    }
