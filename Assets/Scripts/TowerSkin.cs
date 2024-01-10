using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSkin : MonoBehaviour
{
    [SerializeField] private GameObject[] skin;

    [SerializeField] private int towerIndex;

    private bool isEnemy;

    private void Awake()
    {
        skin[0].SetActive(false);

        isEnemy = GetComponentInParent<FieldSlot>().isEnemy;
        if (isEnemy)
        {
            skin[Settings.enemyTowerSkins[towerIndex]].SetActive(true);
        }
        else
        {
            skin[Settings.meTowerSkins[towerIndex]].SetActive(true);
        }

    }

}
