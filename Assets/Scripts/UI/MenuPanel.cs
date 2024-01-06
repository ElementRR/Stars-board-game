using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] GameObject tut;
    [SerializeField] GameObject enemyPanel;
    [SerializeField] GameObject settingsPanel;

    private void Awake()
    {
        Time.timeScale = 1;
    }
    public void EnterTutorial()
    {
        Instantiate(tut);
    }
    public void EnterSettings()
    {
        Instantiate(settingsPanel);
    }

    public void PlayMatch()
    {
        Instantiate(enemyPanel);
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitApp()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }

    public void ResetPoints()
    {
        ScoreManager.score = 0;
        ScoreManager.isEdWon = false;
        PlayerPrefs.DeleteAll();
    }
}
