using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    protected ShowTurnAction showTurnAction;
    [SerializeField] protected List<int> cardsToChooseFrom;
    [SerializeField] protected List<int> cardsToPlay;

    protected bool firstTurn = true;

    [SerializeField] protected GameObject profilePhoto;

    protected void Start()
    {
        GameManager.instance.OnFirstTurnEnd += InsertCard6;
    }

    protected virtual void Awake()
    {
        showTurnAction = GetComponent<ShowTurnAction>();
        firstTurn = true;

        Settings.enemyTowerSkins = new(new int[] { 0, 0, 0, 0, 0 });
    }

    public virtual void EnemyPlay()
    {
    }

    protected void InsertCard6()
    {
        firstTurn = false;
    }

    protected void OnDestroy()
    {
        GameManager.instance.OnFirstTurnEnd -= InsertCard6;
    }
}
