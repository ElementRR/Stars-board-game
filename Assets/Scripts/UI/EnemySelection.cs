using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySelection : TutPanel
{
    [SerializeField] Animator blackPanel;

    public delegate void Outdoor(Enemy.Name enemyName);
    public static event Outdoor OnEnemyChose;

    //enemyIndex = 0 : Ed, 1 : Rick, 2 : Ana
    public void EnemyChose(int enemyIndex)
    {
        OnEnemyChose?.Invoke(Enemy.Name.Ed);
        StartCoroutine(ChangeScene());
    }

    private IEnumerator ChangeScene()
    {
        blackPanel.Play("TransToBlack");
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene(1);
    }
}
