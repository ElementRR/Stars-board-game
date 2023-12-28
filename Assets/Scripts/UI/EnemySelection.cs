using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySelection : TutPanel
{
    [SerializeField] Animator blackPanel;

    //enemyIndex = 0 : Ed, 1 : Rick, 2 : Ana
    public void EnemyChose(int enemyIndex)
    {
        StartCoroutine(ChangeScene());
    }

    private IEnumerator ChangeScene()
    {
        blackPanel.Play("TransToBlack");
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene(1);
    }
}
