using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] GameObject window1;
    [SerializeField] GameObject enemyPanel;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject shopPanel;

    [Header("Sound FX")]
    AudioSource audioSource;
    [SerializeField] AudioClip UIclick;

    [SerializeField] Animator blackPanel;

    private void Awake()
    {
        Time.timeScale = 1;
        audioSource = GetComponent<AudioSource>();
    }
    public void EnterTutorial()
    {
        audioSource.PlayOneShot(UIclick);
        Instantiate(window1);
    }
    public void EnterSettings()
    {
        audioSource.PlayOneShot(UIclick);
        Instantiate(settingsPanel);
    }
    public void EnterShop()
    {
        audioSource.PlayOneShot(UIclick);
        Instantiate(shopPanel);
    }

    public void PlayMatch()
    {
        audioSource.PlayOneShot(UIclick);
        Instantiate(enemyPanel);
    }
    public void BackToMenu()
    {
        audioSource.PlayOneShot(UIclick);
        StartCoroutine(ChangeScene(0));
    }

    public void ExitApp()
    {
        audioSource.PlayOneShot(UIclick);
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
        audioSource.PlayOneShot(UIclick);
        ScoreManager.score = 0;
        ScoreManager.isEdWon = false;
        PlayerPrefs.DeleteAll();
    }
}
