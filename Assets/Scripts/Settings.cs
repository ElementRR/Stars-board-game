using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public static Settings instance;
    public static float musicVolume = 0.5f;
    [SerializeField] private AudioListener AudioListener;

    public static bool isFirstTimePlaying = true;

    public static List<int> meTowerSkins = new(new int[] { 0, 0, 0, 0, 0 });
    public static List<int> enemyTowerSkins = new(new int[] { 0, 0, 0, 0, 0 });
    private void Awake()
    {
        instance = this;
        AudioListener = GameObject.Find("Main Camera").GetComponent<AudioListener>();
    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Level Loaded");
        Debug.Log(scene.name);
        Debug.Log(mode);

        AudioListener = GameObject.Find("Main Camera").GetComponent<AudioListener>();
        AudioListener.volume = musicVolume;
    }
    public void UpdateVolume()
    {
        AudioListener.volume = musicVolume;
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
    }
}
