using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject destrFX;

    [Header("Sound FX")]
    public AudioClip installTower;
    [SerializeField] private AudioSource reproduce;

    public float animTime;

    private bool isOffline;

    private void Awake()
    {
        reproduce = GetComponent<AudioSource>();
        reproduce.PlayOneShot(installTower);

        isOffline = GameObject.FindGameObjectWithTag("GameController").TryGetComponent(out GameManager gm);
        Debug.Log("isOffline: " + isOffline);
    }
    private void WaitAnim()
    {
        if(isOffline)
        {
            GameManager.instance.installEnd = false;
        }
        else
        {
            NetworkGM.instance.installEnd = false;
        }
    }
    public void GoAhead()
    {
        if (isOffline)
        {
            GameManager.instance.installEnd = true;
        }
        else
        {
            NetworkGM.instance.installEnd = true;
        }

    }


    public void Destruction()
    {
        Instantiate(destrFX, gameObject.transform.position, destrFX.transform.rotation);
    }
}
