using System.Collections.Generic;
using UnityEngine;

// OBS: The inhibitor sometimes is returning inhibitor cards that are flipped down
// Try to use Lists instead of 2 en_value variables

// adversary = the opponent of the player we are looking
// enemy = the AI player
// me = the human player
public class ShowTurnAction : MonoBehaviour
{
    private bool isGameOver = false;

    public GameObject[] fieldSlots;

    private int me_value1 = 6;   // the index of the card in this fase

    [SerializeField] private List<int> en_value; // the index of the adversary tower

    int whereInstallT;

    [Header("Sound FX")]
    public AudioClip inhTower;
    private AudioSource reproduce;

    private void Awake()
    {
        reproduce = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (fieldSlots[2].GetComponent<FieldSlot>().isFilled && !isGameOver)
        {
            Debug.Log("Player wins!");
            Time.timeScale = 0;
            UIManager.instance.GameOver(false);
            isGameOver = true;
        }
        if (fieldSlots[5].GetComponent<FieldSlot>().isFilled && !isGameOver)
        {
            Debug.Log("AI wins!");
            Time.timeScale = 0;
            UIManager.instance.GameOver(true);
            isGameOver = true;
        }
    }

    private void GetCardAndSlot(int faseNumber, out int slotcard, out GameObject cardSlot1)
    {
        switch (faseNumber)
        {
            case 1:
                slotcard = UIManager.instance.slot1card;
                cardSlot1 = GameManager.instance.cardSlot1;
                break;
            case 2:
                slotcard = UIManager.instance.slot2card;
                cardSlot1 = GameManager.instance.cardSlot2;
                break;
            case 3:
                slotcard = UIManager.instance.slot3card;
                cardSlot1 = GameManager.instance.cardSlot3;
                break;
            case 4:
                slotcard = UIManager.instance.slot4card;
                cardSlot1 = GameManager.instance.cardSlot4;
                break;
            case 5:
                slotcard = UIManager.instance.slot5card;
                cardSlot1 = GameManager.instance.cardSlot5;
                break;
            case 6:
                slotcard = UIManager.instance.slot6card;
                cardSlot1 = GameManager.instance.cardSlot6;
                break;
            default:
                slotcard = 7;
                cardSlot1 = GameManager.instance.cardSlot1;
                break;
        }
    }

    public void ActionInShowTurn(bool isEnemy, int faseNumber)
    {
        bool adversaryHasAnt;
        en_value = new List<int>(0);

        // me or enemy?
        if (isEnemy)
        {
            whereInstallT = 3;
        }
        else
        {
            whereInstallT = 0;
        }

        GameObject cardSlot1;
        GetCardAndSlot(faseNumber, out int slotcard, out cardSlot1); // Fases 1.2 = 4, 2.2 = 5, 3.2 = 6

        int adversarySlot1 = isEnemy ? 0 : 3;
        int adversarySlot2 = isEnemy ? 1 : 4;

        adversaryHasAnt = (fieldSlots[adversarySlot1].GetComponent<FieldSlot>().isFilled ||
            fieldSlots[adversarySlot2].GetComponent<FieldSlot>().isFilled) ? true : false;

        me_value1 = slotcard;
        // tower or inhibitor?
        if (me_value1 < 4)
        {
            //  tower: adversary has antagonist?
            if (adversaryHasAnt) // yes : antagonist interacts with tower?
            {
                CheckAdversarySlots(isEnemy);

                CheckReturnCard(cardSlot1, isEnemy); // yes: return card
                en_value.Clear();
            }
            else // no : install tower
            {
                InstallTower(whereInstallT, cardSlot1);
                en_value.Clear();
            }
        }
        else //  inhibitor: adversary has tower?
        {
            if (!adversaryHasAnt) // no : return inhibitor card
            {
                UIManager.instance.ReturnCard(me_value1, cardSlot1, isEnemy);
                en_value.Clear();
            }
            else // yes : tower interacts with inhibitor?
            {
                CheckAdversarySlots(isEnemy);

                DestroyAdversaryTower(isEnemy); // yes: destroy 1 enemy tower

                UIManager.instance.ReturnCard(me_value1, cardSlot1, isEnemy);
                me_value1 = 6;

                en_value.Clear();
            }
        }
    }


    private void CheckAdversarySlots(bool isEnemy) // check if the adversary slots are filled and what tower is
    {
        int adversarySlot = isEnemy ? 0 : 3;

        for (int i = 0; i < 2; i++)
        {
            if (fieldSlots[adversarySlot + i].GetComponent<FieldSlot>().isFilled)
            {
                en_value.Add(fieldSlots[adversarySlot + i].GetComponent<FieldSlot>().towerToInstantiate);
            }
        }

    }
    private void CheckReturnCard(GameObject cardSlot, bool isEnemy)
    {
        if (me_value1 == 0 && en_value.Contains(1))
        {
            UIManager.instance.ReturnCard(me_value1, cardSlot, isEnemy);
            reproduce.PlayOneShot(inhTower);
        }
        else if (me_value1 == 1 && en_value.Contains(0))
        {
            UIManager.instance.ReturnCard(me_value1, cardSlot, isEnemy);
            reproduce.PlayOneShot(inhTower);
        }
        else if (me_value1 == 2 && en_value.Contains(3))
        {
            UIManager.instance.ReturnCard(me_value1, cardSlot, isEnemy);
            reproduce.PlayOneShot(inhTower);
        }
        else if (me_value1 == 3 && en_value.Contains(2))
        {
            UIManager.instance.ReturnCard(me_value1, cardSlot, isEnemy);
            reproduce.PlayOneShot(inhTower);
        }
        else
        {
            InstallTower(whereInstallT, cardSlot);
        }
    }
    private void InstallTower(int slot, GameObject cardSlot)
    {
        for (int i = 0; i <= 2; i++)
        {
            if (!fieldSlots[slot + i].GetComponent<FieldSlot>().isFilled)
            {
                fieldSlots[slot + i].GetComponent<FieldSlot>().towerToInstantiate = me_value1;
                fieldSlots[slot + i].GetComponent<FieldSlot>().InstantiateInSlot();
                Debug.Log("Tower installed in slot " + (slot + i));
                break;
            }
        }

        if (cardSlot.transform.childCount > 0)
        {
            Destroy(cardSlot.transform.GetChild(0).gameObject);
        }

        me_value1 = 6;
    }
    private void DestroyAdversaryTower(bool isEnemy)
    {
        if (!isEnemy)
        {
            if (me_value1 == 4 && (en_value.Contains(1) || en_value.Contains(3)))
            {
                // Destroy 1 enemy cold tower
                Destroy1InhTower(3, 1);
            }
            else if (me_value1 == 5 && (en_value.Contains(0) || en_value.Contains(2)))
            {
                // Destroy 1 enemy hot tower
                Destroy1InhTower(3, 0);
            }
        }
        else
        {
            if (me_value1 == 4 && (en_value.Contains(1) || en_value.Contains(3)))
            {
                // Destroy 1 me cold tower
                Destroy1InhTower(0, 1);

                // and return me card

                if (en_value.Count == 2 && en_value[1] is (1 or 3))
                {
                    UIManager.instance.cardIndex[en_value[1]].SetActive(true);
                }
                else if (en_value[0] is (1 or 3))
                {
                    UIManager.instance.cardIndex[en_value[0]].SetActive(true);
                }
            }
            else if (me_value1 == 5 && (en_value.Contains(0) || en_value.Contains(2)))
            {
                // Destroy 1 me hot tower
                Destroy1InhTower(0, 0);

                // and return me card
                if (en_value.Count == 2 && en_value[1] is (0 or 2))
                {
                    UIManager.instance.cardIndex[en_value[1]].SetActive(true);

                }
                else if (en_value[0] is (0 or 2))
                {
                    UIManager.instance.cardIndex[en_value[0]].SetActive(true);
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

            if (fieldSlots[minSlot + 1].transform.childCount > 0)
            {
                reproduce.PlayOneShot(inhTower);
                Destroy(fieldSlots[minSlot + 1].transform.GetChild(0).gameObject);
            }

        }
        else
        {
            fieldSlots[minSlot].GetComponent<FieldSlot>().towerToInstantiate = 6;
            fieldSlots[minSlot].GetComponent<FieldSlot>().isFilled = false;
            if (fieldSlots[minSlot].transform.childCount > 0)
            {
                reproduce.PlayOneShot(inhTower);
                Destroy(fieldSlots[minSlot].transform.GetChild(0).gameObject);
            }
        }
    }
}
