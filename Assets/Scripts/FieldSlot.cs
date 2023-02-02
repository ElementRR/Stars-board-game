using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldSlot : MonoBehaviour
{
    public int slotNumber;
    public GameObject[] towerIndex;
    public int towerToInstantiate;
    public bool isFilled;

    public void InstantiateInSlot()
    {
        Instantiate(towerIndex[towerToInstantiate], transform.position, transform.rotation, gameObject.transform);
        isFilled = true;
    }
}
