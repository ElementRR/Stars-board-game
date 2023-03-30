using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSecs : MonoBehaviour
{
    public float time = 0.5f;
    void Awake()
    {
        Destroy(gameObject, time);
    }
}
