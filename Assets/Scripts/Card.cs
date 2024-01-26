using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int index;

    private ParticleSystem starParticle;
    [SerializeField] private GameObject swordFX;
    [SerializeField] private float timeSword = 0.8f;
    private void Awake()
    {
        starParticle = GetComponentInChildren<ParticleSystem>();
    }

    public void OnParticleTrigger()
    {
         if(index == 7) { starParticle.Play(); }
         else if(index == 5)
         {
            StartCoroutine(InvokeSword(timeSword));
         }
    }

    private IEnumerator InvokeSword(float t)
    {
        Vector3 offset = new Vector3(0, 0.5f, -1.5f);

        yield return new WaitForSeconds(t);
        Instantiate(swordFX, gameObject.transform.position + offset, gameObject.transform.rotation);
    }
}
