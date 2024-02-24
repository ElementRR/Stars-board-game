using UnityEngine;

public class TutPanel : MonoBehaviour
{
    [SerializeField] protected GameObject tut;
    [SerializeField] protected GameObject[] tutPanels;
    [SerializeField] protected int panel = 0;

    [Header("Sound FX")]
    protected AudioSource audioSource;
    [SerializeField] protected AudioClip UIclick;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void NextPanel()
    {
        if (panel < tutPanels.Length - 1)
        {
            audioSource.PlayOneShot(UIclick);
            tutPanels[panel + 1].SetActive(true);
            panel++;
        }
        else if (panel >= tutPanels.Length - 1)
        {
            audioSource.PlayOneShot(UIclick); ;
            panel = 0;
            Destroy(tut);
        }
    }
    public void BackPanel()
    {
        if (panel > 0)
        {
            audioSource.PlayOneShot(UIclick);
            tutPanels[panel].SetActive(false);
            panel--;
        }

        if (panel <= 0)
        {
            audioSource.PlayOneShot(UIclick);
            tutPanels[0].SetActive(false);
            panel = 0;
            Destroy(tut);
        }
    }
    public void NoMoreFirstTime()
    {
          Settings.isFirstTimePlaying = false;
        PlayerPrefs.SetInt("isFirstTime", 0);
    }
}
