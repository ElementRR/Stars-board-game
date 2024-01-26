using UnityEngine;

public class TutPanel : MonoBehaviour
{
    [SerializeField] protected GameObject tut;
    [SerializeField] protected GameObject[] tutPanels;
    [SerializeField] protected int panel = 0;

    public void NextPanel()
    {
        if (panel < tutPanels.Length - 1)
        {
            //tutPanels[panel].SetActive(false);
            tutPanels[panel + 1].SetActive(true);
            panel++;
        }
        else if (panel >= tutPanels.Length - 1)
        {
            //tutPanels[^1].SetActive(false);
            panel = 0;
            Destroy(tut);
        }
    }
    public void BackPanel()
    {
        if (panel > 0)
        {
            tutPanels[panel].SetActive(false);
            //tutPanels[panel - 1].SetActive(true);
            panel--;
        }

        if (panel <= 0)
        {
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
