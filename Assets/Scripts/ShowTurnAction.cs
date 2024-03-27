using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Transform[] cameraLocs;

    public GameObject mainCamera;

    private int towerCost = 3;

    private int inhCost = 1;

    [Header("Sound FX")]
    [SerializeField] private AudioClip inhTower;
    private AudioSource reproduce;

    public delegate void Outdoor(string message);
    public static event Outdoor OnMessageSent;

    public delegate void Sword(Vector3 en_position);
    public static event Sword OnEn_positionLoc;

    private void Awake()
    {
        reproduce = GetComponent<AudioSource>();
        isGameOver = false;
    }

    private void GetCardAndSlot(int faseNumber, out int slotcard, out GameObject cardSlot1)
    {
        if(TryGetComponent(out GameManager gm))
        {
            cardSlot1 = gm.cardSlots[faseNumber - 1];
        }
        else
        {
            cardSlot1 = NetworkGM.instance.cardSlots[faseNumber - 1];
        }

        slotcard = cardSlot1.GetComponentInChildren<Card>().index;
    }

    public void ActionInShowTurn(bool isEnemy, int faseNumber)
    {
        bool isOffline;
        isOffline = TryGetComponent(out GameManager gm);

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
        }
        else if (me_value1 > 3 && me_value1 < 6) //  inhibitor: adversary has tower?
        {
            int stars;

            if (isOffline)
            {
                stars = (whereInstallT == 3) ? GameManager.enemyStars : GameManager.meStars;
            }
            else
            {
                stars = (whereInstallT == 3) ? NetworkGM.enemyStars : NetworkGM.meStars;
            }

            if (stars < inhCost) // inhibitor: do you have enough stars?
            {
                if (isOffline)
                {
                    UIManager.instance.ReturnCard(me_value1, cardSlot1, isEnemy);
                }
                else
                {
                    NetworkUI.instance.ReturnCard(me_value1, cardSlot1);
                }

                NotEnoughStars();
            }
            else // stars requisition: success
            {
                switch ((isEnemy ? 1 : 0) + (isOffline ? 2 : 0))
                {
                    case 3: // isEnemy = true, isOffline = true
                        GameManager.enemyStars -= inhCost;
                        UIManager.instance.enemyStarCount.text = "" + GameManager.enemyStars;
                        break;
                    case 2: // isEnemy = false, isOffline = true
                        GameManager.meStars -= inhCost;
                        UIManager.instance.starCount.text = "" + GameManager.meStars;
                        break;
                    case 1: // isEnemy = true, isOffline = false
                        NetworkGM.enemyStars -= inhCost;
                        NetworkUI.instance.enemyStarCount.text = "" + NetworkGM.enemyStars;
                        break;
                    case 0: // isEnemy = false, isOffline = false
                        NetworkGM.meStars -= inhCost;
                        NetworkUI.instance.starCount.text = "" + NetworkGM.meStars;
                        break;
                    default:
                        Debug.Log("Stars number instance not found");
                        break;
                }

                if (!adversaryHasAnt) // no : return inhibitor card
                {
                    reproduce.PlayOneShot(inhTower);
                    OnMessageSent?.Invoke("Inhibitor did not work! -1 star!");
                    if (isOffline)
                    {
                        UIManager.instance.ReturnCard(me_value1, cardSlot1, isEnemy);
                    }
                    else
                    {
                        NetworkUI.instance.ReturnCard(me_value1, cardSlot1);
                    }

                    en_value.Clear();
                }
                else // yes : tower interacts with inhibitor?
                {
                    CheckAdversarySlots(isEnemy);

                    DestroyAdversaryTower(isEnemy); // yes: destroy 1 enemy tower

                    if (isOffline)
                    {
                        UIManager.instance.ReturnCard(me_value1, cardSlot1, isEnemy);
                    }
                    else
                    {
                        NetworkUI.instance.ReturnCard(me_value1, cardSlot1);
                    }

                    me_value1 = blankIndex;

                    en_value.Clear();
                }
            }
        }
        else
        {
            switch ((isEnemy ? 1 : 0) + (isOffline ? 2 : 0))
            {
                case 3: // isEnemy = true, isOffline = true
                    GameManager.enemyStars += 2;
                    UIManager.instance.enemyStarCount.text = "" + GameManager.enemyStars;
                    OnMessageSent?.Invoke("+2 Stars for enemy!");
                    break;
                case 2: // isEnemy = false, isOffline = true
                    GameManager.meStars += 2;
                    UIManager.instance.starCount.text = "" + GameManager.meStars;
                    OnMessageSent?.Invoke("+2 Stars for you!");
                    break;
                case 1: // isEnemy = true, isOffline = false
                    NetworkGM.enemyStars += 2;
                    NetworkUI.instance.enemyStarCount.text = "" + NetworkGM.enemyStars;
                    OnMessageSent?.Invoke("+2 Stars for guest!");
                    break;
                case 0: // isEnemy = false, isOffline = false
                    NetworkGM.meStars += 2;
                    NetworkUI.instance.starCount.text = "" + NetworkGM.meStars;
                    OnMessageSent?.Invoke("+2 Stars for host!");
                    break;
                default:
                    // Handle unexpected cases
                    break;
            }
            DestroyChildObject(cardSlot1);
            
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
        int blockTowerIndex = 3; // dummy value
        bool failBellTower = false;

        switch (me_value1)
        {
            case 0:
                if (en_value.Contains(1)) 
                {
                    message = "Fire tower could not be installed!";
                    blockTowerIndex = en_value.IndexOf(1);
                    shouldReturnCard = true;
                }
                break;
            case 1:
                if (en_value.Contains(0)) 
                {
                    message = "Water tower could not be installed!";
                    blockTowerIndex = en_value.IndexOf(0);
                    shouldReturnCard = true;
                }
                break;
            case 2:
                if (en_value.Contains(3)) 
                {
                    message = "Sun tower could not be installed!";
                    blockTowerIndex = en_value.IndexOf(3);
                    shouldReturnCard = true;
                }
                break;
            case 3:
                if (en_value.Contains(2)) 
                {
                    message = "Moon tower could not be installed!";
                    blockTowerIndex = en_value.IndexOf(2);
                    shouldReturnCard = true;
                }
                break;
            case 6:
                if (en_value.Count > 0) 
                {
                    message = "Bell tower could not be installed!";
                    failBellTower = true;
                    shouldReturnCard = true;
                }
                break;
            default:
                shouldReturnCard = false;
                break;
        }

        if (shouldReturnCard)
        {
            if(TryGetComponent(out GameManager gm))
            {
                UIManager.instance.ReturnCard(me_value1, cardSlot, isEnemy);
            }
            else
            {
                NetworkUI.instance.ReturnCard(me_value1, cardSlot);
            }

            if (failBellTower)
            {
                for (int i = 0; i < 2; i++) 
                {
                    switch (isEnemy)
                    {
                        case false when fieldSlots[i + 3].GetComponent<FieldSlot>().isFilled:
                            fieldSlots[i + 3].GetComponent<FieldSlot>().FailedToInstall();
                            break;
                        case true when fieldSlots[i].GetComponent<FieldSlot>().isFilled:
                            fieldSlots[i].GetComponent<FieldSlot>().FailedToInstall();
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                switch (isEnemy)
                {
                    case false when fieldSlots[blockTowerIndex + 3].GetComponent<FieldSlot>().isFilled:
                        fieldSlots[blockTowerIndex + 3].GetComponent<FieldSlot>().FailedToInstall();
                        break;
                    case true when fieldSlots[blockTowerIndex].GetComponent<FieldSlot>().isFilled:
                        fieldSlots[blockTowerIndex].GetComponent<FieldSlot>().FailedToInstall();
                        break;
                    default:
                        break;
                }
            }

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
        bool isOffline;
        isOffline = TryGetComponent(out GameManager gm);

        int stars;

        if (isOffline)
        {
            stars = (whereInstallT == 3) ? GameManager.enemyStars : GameManager.meStars;
        }
        else
        {
            stars = (whereInstallT == 3) ? NetworkGM.enemyStars : NetworkGM.meStars;
        }

        if (stars < towerCost)
        {
            if(isOffline)
            {
                UIManager.instance.ReturnCard(me_value1, cardSlot, (whereInstallT == 3) ? true : false);
            }
            else
            {
                NetworkUI.instance.ReturnCard(me_value1, cardSlot);
            }
            
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

        //cameras[slot < 3 ? 1 : 2].SetActive(true);
        mainCamera.transform.SetPositionAndRotation(cameraLocs[slot < 3 ? 1 : 2].position,
            cameraLocs[slot < 3 ? 1 : 2].rotation);

        DestroyChildObject(cardSlot);

        switch (whereInstallT + (isOffline ? 0 : 1))
        {
            case 0:
                GameManager.meStars -= towerCost;
                UIManager.instance.starCount.text = "" + GameManager.meStars;
                break;

            case 3:
                GameManager.enemyStars -= towerCost;
                UIManager.instance.enemyStarCount.text = "" + GameManager.enemyStars;
                break;
            case 1:
                NetworkGM.meStars -= towerCost;
                NetworkUI.instance.starCount.text = "" + NetworkGM.meStars;
                break;

            case 4:
                NetworkGM.enemyStars -= towerCost;
                NetworkUI.instance.enemyStarCount.text = "" + NetworkGM.enemyStars;
                break;
            default:
                Debug.Log("Error to subtract stars");
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
            StartCoroutine(parent.transform.GetComponentInChildren<Card>().DestroySequence());
            //Destroy(parent.transform.GetChild(0).gameObject);
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
        bool isOffline;
        isOffline = TryGetComponent(out GameManager gm);

        switch ((isEnemy ? 0 : 1) + (isOffline ? 0 : 2))
        {
            case 0: //isEnemy and isOffline
                if (me_value1 == 4 && (en_value.Contains(1) || en_value.Contains(3)))
                {
                    // Destroy 1 me cold tower
                    Destroy1InhTower(0, 1);

                    // and return me card

                    if (en_value.Count == 2 && en_value[1] is (1 or 3))
                    {
                        UIManager.instance.ReturnCard(en_value[1]);
                    }
                    else if (en_value[0] is (1 or 3))
                    {
                        UIManager.instance.ReturnCard(en_value[0]);
                    }
                }
                else if (me_value1 == 5 && (en_value.Contains(0) || en_value.Contains(2)))
                {
                    // Destroy 1 me hot tower
                    Destroy1InhTower(0, 0);

                    // and return me card
                    if (en_value.Count == 2 && en_value[1] is (0 or 2))
                    {
                        UIManager.instance.ReturnCard(en_value[1]);

                    }
                    else if (en_value[0] is (0 or 2))
                    {
                        UIManager.instance.ReturnCard(en_value[0]);
                    }
                }
                break;
            case 1: //isMe and isOffline
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
                break;
            case 2: // isGuest and !isOffline
                if (me_value1 == 4 && (en_value.Contains(1) || en_value.Contains(3)))
                {
                    // Destroy 1 cold tower
                    Destroy1InhTower(0, 1);

                    // and return card

                    if (en_value.Count == 2 && en_value[1] is (1 or 3))
                    {
                        NetworkUI.instance.ReturnCard(en_value[1], true);
                    }
                    else if (en_value[0] is (1 or 3))
                    {
                        NetworkUI.instance.ReturnCard(en_value[0], true);
                    }
                }
                else if (me_value1 == 5 && (en_value.Contains(0) || en_value.Contains(2)))
                {
                    // Destroy 1 hot tower
                    Destroy1InhTower(0, 0);

                    // and return card
                    if (en_value.Count == 2 && en_value[1] is (0 or 2))
                    {
                        NetworkUI.instance.ReturnCard(en_value[1], true);

                    }
                    else if (en_value[0] is (0 or 2))
                    {
                        NetworkUI.instance.ReturnCard(en_value[0], true);
                    }
                }
                break;
            case 3: // isHost and !isOffline
                if (me_value1 == 4 && (en_value.Contains(1) || en_value.Contains(3)))
                {
                    // Destroy 1 cold tower
                    Destroy1InhTower(3, 1);

                    // and return card

                    if (en_value.Count == 2 && en_value[1] is (1 or 3))
                    {
                        NetworkUI.instance.ReturnCard(en_value[1], true);
                    }
                    else if (en_value[0] is (1 or 3))
                    {
                        NetworkUI.instance.ReturnCard(en_value[0], true);
                    }
                }
                else if (me_value1 == 5 && (en_value.Contains(0) || en_value.Contains(2)))
                {
                    // Destroy 1 hot tower
                    Destroy1InhTower(3, 0);

                    // and return card
                    if (en_value.Count == 2 && en_value[1] is (0 or 2))
                    {
                        NetworkUI.instance.ReturnCard(en_value[1], true);

                    }
                    else if (en_value[0] is (0 or 2))
                    {
                        NetworkUI.instance.ReturnCard(en_value[0], true);
                    }
                }
                break;

            default:
                Debug.Log("Error when trying to inhibit opposite tower.");
                break;
        }
    }

    private void Destroy1InhTower(int minSlot, int hotNcoldValue)
    {
        if (fieldSlots[minSlot + 1].GetComponent<FieldSlot>().isFilled &&
                    (fieldSlots[minSlot + 1].GetComponent<FieldSlot>().towerToInstantiate == hotNcoldValue ||
                    fieldSlots[minSlot + 1].GetComponent<FieldSlot>().towerToInstantiate == hotNcoldValue + 2))
        {
            // Give tower's position to sword
            OnEn_positionLoc?.Invoke(fieldSlots[minSlot + 1].transform.position);

            fieldSlots[minSlot + 1].GetComponent<FieldSlot>().towerToInstantiate = blankIndex;
            fieldSlots[minSlot + 1].GetComponent<FieldSlot>().isFilled = false;

            if (fieldSlots[minSlot + 1].transform.childCount > 0)
            {
                StartCoroutine(InhibitSequence(minSlot + 1));
            }

        }
        else if(fieldSlots[minSlot].GetComponent<FieldSlot>().isFilled)
        {
            // Give tower's position to sword
            OnEn_positionLoc?.Invoke(fieldSlots[minSlot].transform.position);

            fieldSlots[minSlot].GetComponent<FieldSlot>().towerToInstantiate = blankIndex;
            fieldSlots[minSlot].GetComponent<FieldSlot>().isFilled = false;
            if (fieldSlots[minSlot].transform.childCount > 0)
            {
                StartCoroutine(InhibitSequence(minSlot));
            }
        }
    }
    private IEnumerator InhibitSequence(int slot)
    {
        yield return new WaitForSeconds(0.3f);
        OnMessageSent?.Invoke("Tower was destroyed");
        fieldSlots[slot].transform.GetComponentInChildren<Tower>().Destruction();

        Destroy(fieldSlots[slot].transform.GetChild(0).gameObject);
    }

    private IEnumerator EndGameRoutine(bool enemyWins)
    {
        yield return new WaitForSeconds(1.5f);

        if(TryGetComponent(out GameManager gm))
        {
            UIManager.instance.GameOver(enemyWins);
        }
        else
        {
            NetworkUI.instance.GameOver(enemyWins);
        }

        isGameOver = true;
        Time.timeScale = 0;

    }
}
