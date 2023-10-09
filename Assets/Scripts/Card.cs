using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int index;

    private ParticleSystem starParticle;

    private void Awake()
    {
        starParticle = GetComponentInChildren<ParticleSystem>();  
    }

    public void OnParticleTrigger()
    {
         if(index == 7) { starParticle.Play(); }
    }
}
