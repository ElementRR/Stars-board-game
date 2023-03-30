using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchMusic : MonoBehaviour
{
    private AudioSource source;
    public AudioClip[] musics;
    public float volume = 0.5f;

    // Start is called before the first frame update
    void Awake()
    {
        source = GetComponent<AudioSource>();
        source.PlayOneShot(GetAudioClip(), volume);
        source.loop = true;
    }

    public AudioClip GetAudioClip()
    {
        int random = Random.Range(0, musics.Length);
        return musics[random];
    }
}
