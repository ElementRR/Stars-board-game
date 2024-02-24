using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] GameObject window1;
    [SerializeField] GameObject enemyPanel;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject shopPanel;
    [SerializeField] GameObject resetPanel;

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
    public void EnterReset()
    {
        audioSource.PlayOneShot(UIclick);
        Instantiate(resetPanel);
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

    public void ResetPoints() => StartCoroutine(ResetRoutine());
    private IEnumerator ResetRoutine()
    {
        audioSource.PlayOneShot(UIclick);
        ScoreManager.score = 0;
        ScoreManager.isEdWon = false;
        Settings.isFirstTimePlaying = true;
        PlayerPrefs.DeleteAll();

        yield return new WaitForSeconds(1.1f);
        
        Destroy(gameObject);
    }
}
