using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] GameObject window1;
    [SerializeField] GameObject enemyPanel;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject shopPanel;

    [SerializeField] Animator blackPanel;

    private void Awake()
    {
        Time.timeScale = 1;
    }
    public void EnterTutorial()
    {
        Instantiate(window1);
    }
    public void EnterSettings()
    {
        Instantiate(settingsPanel);
    }
    public void EnterShop()
    {
        Instantiate(shopPanel);
    }

    public void PlayMatch()
    {
        Instantiate(enemyPanel);
    }
    public void BackToMenu()
    {
        StartCoroutine(ChangeScene(0));
    }

    public void ExitApp()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }

    private IEnumerator ChangeScene(int scene)
    {
        blackPanel.Play("TransToBlack");
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene(scene);
    }

    public void ResetPoints()
    {
        ScoreManager.score = 0;
        ScoreManager.isEdWon = false;
        PlayerPrefs.DeleteAll();
    }
}
