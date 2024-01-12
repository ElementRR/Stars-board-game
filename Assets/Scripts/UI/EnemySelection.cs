using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySelection : TutPanel
{
    [SerializeField] Animator blackPanel;

    public delegate void Outdoor(Enemy.Name enemyName);
    public static event Outdoor OnEnemyChose;

    [SerializeField] private GameObject textRoot1;
    [SerializeField] private GameObject textRoot2;

    [Header("Erase later")]
    [SerializeField] int FalseScore;

    private void Awake()
    {
        ScoreManager.instance.AddScore(FalseScore);

        if (ScoreManager.isEdWon == true)
        {
            Destroy(textRoot1);
        }

        if (ScoreManager.score >= 400)
        {
            Destroy(textRoot2 );
        }
    }

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
