using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject destrFX;

    public void Destruction()
    {
        Instantiate(destrFX, gameObject.transform.position, destrFX.transform.rotation);
    }
}
