using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] GameObject tut;
    [SerializeField] Animator blackPanel;

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
    public void GoToLink()
    {
        Application.OpenURL("https://forms.gle/NTZWU9CJ3i9fGLocA");
    }

    private IEnumerator ChangeScene()
    {
        blackPanel.Play("TransToBlack");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(1);
    }
}
