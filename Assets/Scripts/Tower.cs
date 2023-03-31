using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject destrFX;

    private void WaitAnim()
    {
        GameManager.instance.installEnd = false;
    }
    private void GoAhead()
    {
        GameManager.instance.installEnd = true;
    }

    public void Destruction()
    {
        Instantiate(destrFX, gameObject.transform.position, destrFX.transform.rotation);
    }
}
