using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] GameObject tut;
    [SerializeField] Animator blackPanel;

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
        StartCoroutine(ChangeScene());
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
    public void GoToLink()
    {
        Application.OpenURL("https://forms.gle/NTZWU9CJ3i9fGLocA");
    }

    private IEnumerator ChangeScene()
    {
        blackPanel.Play("TransToBlack");
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene(1);
    }
}
