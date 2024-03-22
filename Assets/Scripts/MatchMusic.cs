using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class MatchMusic : MonoBehaviour
{
    private AudioSource source;
    public AudioClip[] musics;
    public AudioClip tensionSong;
    public float volume = 0.5f;

    // Start is called before the first frame update
    void Awake()
    {
        source = GetComponent<AudioSource>();
        StartCoroutine(MusicOrder());
    }

    public AudioClip GetAudioClip()
    {
        int random = Random.Range(0, musics.Length - 1);
        return musics[random];
    }
    private IEnumerator MusicOrder()
    {
        source.loop = false;
        source.volume = volume;
        source.clip = tensionSong;
        source.PlayDelayed(1);
        yield return new WaitUntil(() => !source.isPlaying);

        if (GameObject.Find("GameManager").GetComponent<GameManager>())
        {
            GameManager.instance.GetEnemyAI();

            if (GameManager.enemyIndex == Enemy.Name.AngryAna)
            {
                source.clip = musics[^1];
                source.loop = true;
                source.Play();
            }
            else
            {
                source.clip = GetAudioClip();
                source.loop = true;
                source.Play();
            }
        }
        else
        {
            source.clip = GetAudioClip();
            source.loop = true;
            source.Play();
        }
    }
}
