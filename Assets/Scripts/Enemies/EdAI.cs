using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdAI : EnemyAI
{
    // Insert specific logic for Ed
    protected override void Awake()
    {
        showTurnAction = GetComponent<ShowTurnAction>();
        EnemyPlay();
        firstTurn = true;

        if (ScoreManager.instance._enemyName != Enemy.Name.Ed)
        {
            GetComponent<EdAI>().enabled = false;
        }
    }

}
