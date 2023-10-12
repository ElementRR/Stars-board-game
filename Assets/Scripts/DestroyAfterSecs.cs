using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSecs : MonoBehaviour
{
    public float time = 0.5f;

    public bool fade = false;

    private Material mat;

    private float originalOp;

    void Awake()
    {
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
