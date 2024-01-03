using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] GameObject tut;
    [SerializeField] GameObject enemyPanel;

    private void Awake()
    {
        Time.timeScale = 1;
    }
    public void EnterTutorial()
    {
        Instantiate(tut);
    }

    public void PlayMatch()
    {
        Instantiate(enemyPanel);
        //StartCoroutine(ChangeScene());
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

}
