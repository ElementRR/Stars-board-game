using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// OBS: The inhibitor sometimes is returning inhibitor cards that are flipped down
// Try to use Lists instead of 2 en_value variables

// adversary = the opponent of the player we are looking
// enemy = the AI player
// me = the human player
public class ShowTurnAction : MonoBehaviour
{
    private static bool isGameOver = false;

    public GameObject[] fieldSlots;

    private const int blankIndex = 8;

    private int me_value1 = blankIndex;   // the index of the card in this fase

    [SerializeField] private List<int> en_value; // the index of the adversary tower

    int whereInstallT; // This will show us if is me time (0) or enemy time (3)

    public GameObject[] cameras;

    private int towerCost = 3;

    private int inhCost = 1;

    [Header("Sound FX")]
    public AudioClip inhTower;
    private AudioSource reproduce;

    public delegate void Outdoor(string message);
    public static event Outdoor OnMessageSent;

    private void Awake()
    {
        reproduce = GetComponent<AudioSource>();
        isGameOver = false;
    }

    private void GetCardAndSlot(int faseNumber, out int slotcard, out GameObject cardSlot1)
    {
        cardSlot1 = GameManager.instance.cardSlots[faseNumber - 1];
        slotcard = cardSlot1.GetComponentInChildren<Card>().index;
    }

    public void ActionInShowTurn(bool isEnemy, int faseNumber)
    {
        bool adversaryHasAnt;
        en_value = new List<int>(0);

        // me or enemy?
        whereInstallT = (isEnemy) ? 3 : 0;

        GetCardAndSlot(faseNumber, out int slotcard, out GameObject cardSlot1); // Fases 1.2 = 4, 2.2 = 5, 3.2 = 6

        int adversarySlot1 = isEnemy ? 0 : 3;
        int adversarySlot2 = isEnemy ? 1 : 4;

        adversaryHasAnt = (fieldSlots[adversarySlot1].GetComponent<FieldSlot>().isFilled ||
            fieldSlots[adversarySlot2].GetComponent<FieldSlot>().isFilled) ? true : false;

        me_value1 = slotcard;
        // tower or inhibitor?
        if (me_value1 < 4 || me_value1 == 6)
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
        }else if (me_value1 > 3 && me_value1 < 6) //  inhibitor: adversary has tower?
        {
            int stars = (whereInstallT == 3) ? GameManager.instance.enemyStars : GameManager.instance.meStars;

            if (stars < inhCost)
            {
                
                UIManager.instance.ReturnCard(me_value1, cardSlot1, isEnemy);

                NotEnoughStars();
            }
            else
            {
                if (isEnemy)
                {
                    GameManager.instance.enemyStars -= inhCost;
                    UIManager.instance.enemyStarCount.text = "" + GameManager.instance.enemyStars;
                }
                else
                {
                    GameManager.instance.meStars -= inhCost;
                    UIManager.instance.starCount.text = "" + GameManager.instance.meStars;
                }

                if (!adversaryHasAnt) // no : return inhibitor card
                {
                    reproduce.PlayOneShot(inhTower);
                    OnMessageSent?.Invoke("Inhibitor did not work!");
                    UIManager.instance.ReturnCard(me_value1, cardSlot1, isEnemy);

                    en_value.Clear();
                }
                else // yes : tower interacts with inhibitor?
                {
                    CheckAdversarySlots(isEnemy);

                    DestroyAdversaryTower(isEnemy); // yes: destroy 1 enemy tower

                    UIManager.instance.ReturnCard(me_value1, cardSlot1, isEnemy);

                    me_value1 = blankIndex;

                    en_value.Clear();
                }
            }
        }
        else
        {
            if (!isEnemy)
            { 
                GameManager.instance.meStars += 2;
                OnMessageSent?.Invoke("+2 Stars for you!");
                UIManager.instance.starCount.text = "" + GameManager.instance.meStars;
            }
            else
            {
                GameManager.instance.enemyStars += 2;
                OnMessageSent?.Invoke("+2 Stars for enemy!");
                //UIManager.instance.enemyStarCount.text = "" + GameManager.instance.enemyStars;
            }
            

            if (cardSlot1.transform.childCount > 0)
            {
                Destroy(cardSlot1.transform.GetChild(0).gameObject);
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
        string message = "";
        bool shouldReturnCard = false;

        switch (me_value1)
        {
            case 0:
                if (en_value.Contains(1)) 
                {
                    message = "Fire tower could not be installed!";
                    shouldReturnCard = true;
                }
                break;
            case 1:
                if (en_value.Contains(0)) 
                {
                    message = "Water tower could not be installed!";
                    shouldReturnCard = true;
                }
                break;
            case 2:
                if (en_value.Contains(3)) 
                {
                    message = "Sun tower could not be installed!";
                    shouldReturnCard = true;
                }
                break;
            case 3:
                if (en_value.Contains(2)) 
                {
                    message = "Moon tower could not be installed!";
                    shouldReturnCard = true;
                }
                break;
            case 6:
                if (en_value.Count > 0) 
                {
                    message = "Bell tower could not be installed!";
                    shouldReturnCard = true;
                }
                break;
            default:
                shouldReturnCard = false;
                break;
        }

        if (shouldReturnCard)
        {
            UIManager.instance.ReturnCard(me_value1, cardSlot, isEnemy);
            reproduce.PlayOneShot(inhTower);
            OnMessageSent?.Invoke(message);
        }
        else
        {
            InstallTower(whereInstallT, cardSlot);
        }
    }

    private void NotEnoughStars()
    {
        reproduce.PlayOneShot(inhTower);
        OnMessageSent?.Invoke("Not enough stars!");
        me_value1 = blankIndex;
        en_value.Clear();
    }

    private void InstallTower(int slot, GameObject cardSlot)
    {
        int stars = (whereInstallT == 3) ? GameManager.instance.enemyStars : GameManager.instance.meStars;

        if (stars < towerCost)
        {
            UIManager.instance.ReturnCard(me_value1, cardSlot, (whereInstallT == 3) ? true : false );
            
            NotEnoughStars();
            
            return;
        }

        for (int i = 0; i <= 2; i++)
        {
            FieldSlot fieldSlot = fieldSlots[slot + i].GetComponent<FieldSlot>();
            if (!fieldSlot.isFilled)
            {
                fieldSlot.towerToInstantiate = me_value1;
                fieldSlot.InstantiateInSlot();
                OnMessageSent?.Invoke("Tower installed!");
                break;
            }
        }

        cameras[slot < 3 ? 1 : 2].SetActive(true);

        DestroyChildObject(cardSlot);

        switch (whereInstallT)
        {
            case 3:
                GameManager.instance.enemyStars -= towerCost;
                //UIManager.instance.enemyStarCount.text = "" + GameManager.instance.enemyStars;
                break;
            default:
                GameManager.instance.meStars -= towerCost;
                UIManager.instance.starCount.text = "" + GameManager.instance.meStars;
                break;
        }

        if (fieldSlots[2].GetComponent<FieldSlot>().isFilled && !isGameOver)
        {
            EndGame("You win!", false);
        }
        if (fieldSlots[5].GetComponent<FieldSlot>().isFilled && !isGameOver)
        {
            EndGame("You Lose", true);
        }
    }

    private void DestroyChildObject(GameObject parent)
    {
        if (parent.transform.childCount != 0)
        {
            Destroy(parent.transform.GetChild(0).gameObject);
        }
    }

    private void EndGame(string message, bool enemyWins)
    {
        OnMessageSent?.Invoke(message);
        StopAllCoroutines();
        StartCoroutine(EndGameRoutine(enemyWins));
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
            fieldSlots[minSlot + 1].GetComponent<FieldSlot>().towerToInstantiate = blankIndex;
            fieldSlots[minSlot + 1].GetComponent<FieldSlot>().isFilled = false;

            if (fieldSlots[minSlot + 1].transform.childCount > 0)
            {
                InhibitSequence(minSlot + 1);
            }

        }
        else
        {
            fieldSlots[minSlot].GetComponent<FieldSlot>().towerToInstantiate = blankIndex;
            fieldSlots[minSlot].GetComponent<FieldSlot>().isFilled = false;
            if (fieldSlots[minSlot].transform.childCount > 0)
            {
                InhibitSequence(minSlot);
            }
        }
    }
    private void InhibitSequence(int slot)
    {
        reproduce.PlayOneShot(inhTower);
        OnMessageSent?.Invoke("Tower was destroyed");
        fieldSlots[slot].transform.GetComponentInChildren<Tower>().Destruction();
        Destroy(fieldSlots[slot].transform.GetChild(0).gameObject);
    }

    private IEnumerator EndGameRoutine(bool enemyWins)
    {
        yield return new WaitForSeconds(1.5f);
        UIManager.instance.GameOver(enemyWins);
        isGameOver = true;
        Time.timeScale = 0;

    }
}
