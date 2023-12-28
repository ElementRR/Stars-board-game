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
        switch (enemyIndex)
        {
            case 0:
                OnEnemyChose?.Invoke(Enemy.Name.Ed);
                break;
            case 1:
                OnEnemyChose?.Invoke(Enemy.Name.Rick);
                break;
            case 2:
                OnEnemyChose?.Invoke(Enemy.Name.Ana);
                break;
            default:
                OnEnemyChose?.Invoke(Enemy.Name.Ed);
                break;
        }

        StartCoroutine(ChangeScene());
    }

    private IEnumerator ChangeScene()
    {
        blackPanel.Play("TransToBlack");
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene(1);
    }
}
