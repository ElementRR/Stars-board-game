using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DestroyAfterSecs : MonoBehaviour
{
    [SerializeField] private float time = 0.5f;

    [SerializeField] private float volume = 1f;

    [SerializeField] private bool fade = false;

    private Material mat;

    private float originalOp;

    [Header("Sound FX")]
    [SerializeField] private new AudioClip audio;
    private AudioSource audioSource;


    void Awake()
    {
        if(GetComponent<AudioSource>() != null)
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.volume = volume;
            audioSource.PlayOneShot(audio);
        }

        if (fade)
        {
            mat = GetComponent<MeshRenderer>().material;
            originalOp = mat.color.a;
        }
        
        Destroy(gameObject, time);
    }

    private void FixedUpdate()
    {
        if (fade)
        {
            FadeNow();
        }
    }

    private void FadeNow()
    {
        Color currentColor = mat.color;
        Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b,
            Mathf.Lerp(currentColor.a, 0, time * Time.deltaTime));
        mat.color = smoothColor;
    }
}
