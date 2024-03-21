using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EnemySelection : TutPanel
{
    [SerializeField] Animator blackPanel;

    public delegate void Outdoor(Enemy.Name enemyName);
    public static event Outdoor OnEnemyChose;

    [SerializeField] private GameObject textRoot1;
    [SerializeField] private GameObject textRoot2;
    [SerializeField] private TMP_InputField inputPass;

    [SerializeField] private AudioClip selectEnemy;

    [Header("Erase later")]
    [SerializeField] int FalseScore;

    protected override void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        ScoreManager.instance.AddScore(FalseScore);

        if (ScoreManager.isEdWon == true)
        {
            Destroy(textRoot1);
        }

        if (ScoreManager.score >= 250)
        {
            Destroy(textRoot2 );
        }
    }

    //enemyIndex = 0 : Ed, 1 : Rick, 2 : Ana
    public void EnemyChose(int enemyIndex)
    {
        audioSource.PlayOneShot(selectEnemy);
        switch (enemyIndex)
        {
            case 0:
                OnEnemyChose?.Invoke(Enemy.Name.Ed);
                StartCoroutine(ChangeScene(1));
                break;
            case 1:
                OnEnemyChose?.Invoke(Enemy.Name.Rick);
                StartCoroutine(ChangeScene(1));
                break;
            case 2:
                OnEnemyChose?.Invoke(Enemy.Name.Ana);
                StartCoroutine(ChangeScene(1));
                break;
            case 3:
                if(inputPass.text == "rato")
                {
                    StartCoroutine(ChangeScene(2));
                }
                else
                {
                    inputPass.text = "Soon!";
                }
                break;
            default:
                OnEnemyChose?.Invoke(Enemy.Name.Ed);
                StartCoroutine(ChangeScene(1));
                break;
        }
    }

    private IEnumerator ChangeScene(int scene)
    {
        blackPanel.Play("TransToBlack");
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene(scene);
    }
}
