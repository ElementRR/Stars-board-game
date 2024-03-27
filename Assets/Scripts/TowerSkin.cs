using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerSkin : MonoBehaviour
{
    [SerializeField] private GameObject[] skin;

    private float animTime = 1f;

    private float timer = 0;

    [SerializeField] private int towerIndex;

    private bool isEnemy;

    private bool isOffline;

    private void Awake()
    {
        skin[0].SetActive(false);

        isOffline = GameObject.FindGameObjectWithTag("GameController").TryGetComponent(out GameManager gm);

        isEnemy = GetComponentInParent<FieldSlot>().isEnemy;

        if (isOffline)
        {
            Debug.Log("Offline skins");
            if (isEnemy)
            {
                skin[Settings.enemyTowerSkins[towerIndex]].SetActive(true);
            }
            else
            {
                skin[Settings.instance.meTowerSkins[towerIndex]].SetActive(true);
            }
        }
        else
        {
            if (isEnemy)
            {
                skin[NetworkGM.instance.enemyTowerSkins[towerIndex]].SetActive(true);
            }
            else
            {
                skin[NetworkGM.instance.meTowerSkins[towerIndex]].SetActive(true);
            }
        }
        animTime = GetComponentInChildren<Tower>().animTime;
    }

    void Update()
    {
        if(timer < animTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            GetComponentInChildren<Tower>().GoAhead();
        }
    }

}
