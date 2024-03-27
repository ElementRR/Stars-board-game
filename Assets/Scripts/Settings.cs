using Alteruna;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public static Settings instance;

    public static bool isFirstTimePlaying = true;

    public List<int> meTowerSkins = new(new int[] { 0, 0, 0, 0, 0 });
    public static List<int> enemyTowerSkins = new(new int[] { 0, 0, 0, 0, 0 });
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        AudioListener.volume = PlayerPrefs.GetFloat("musicVolume");

        if(AudioListener.volume == 0) {
            AudioListener.volume = 0.5f;
        }

        bool intToBool = PlayerPrefs.GetInt("isFirstTime") != 0;
        isFirstTimePlaying = intToBool;

        meTowerSkins[0] = PlayerPrefs.GetInt("FireTower");
        meTowerSkins[1] = PlayerPrefs.GetInt("WaterTower");
        meTowerSkins[3] = PlayerPrefs.GetInt("MoonTower");
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
    }
}
