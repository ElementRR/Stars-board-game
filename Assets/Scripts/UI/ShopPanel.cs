using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShopPanel : TutPanel
{
    [SerializeField] private GameObject[] shopPanels;

    [SerializeField] private GameObject[] fireSelecteds;
    [SerializeField] private GameObject[] waterSelecteds;
    [SerializeField] private GameObject[] moonSelecteds;

    [SerializeField] private GameObject[] payBlockers;

    [SerializeField] private AudioClip selectSkinSound;


    protected override void Awake()
    {
        fireSelecteds[Settings.instance.meTowerSkins[0]].SetActive(true);
        waterSelecteds[Settings.instance.meTowerSkins[1]].SetActive(true);
        moonSelecteds[Settings.instance.meTowerSkins[3]].SetActive(true);

        audioSource = GetComponent<AudioSource>();

        if (ScoreManager.score >= 700)
        {
            for (int i = 0; i < payBlockers.Length; i++)
            {
                payBlockers[i].SetActive(false);
            }
        } else if (ScoreManager.score >= 450)
        {
            for (int i = 0; i < 2; i++)
            {
                payBlockers[i].SetActive(false);
            }
        }else if (ScoreManager.score >= 200)
        {
            payBlockers[0].SetActive(false);
        }

    }

    public void GoToPanel(int index)
    {
        audioSource.PlayOneShot(UIclick);
        shopPanels[0].SetActive(false);
        shopPanels[index].SetActive(true);
    }
    public void BackToMain(int index)
    {
        audioSource.PlayOneShot(UIclick);
        shopPanels[index].SetActive(false);
        shopPanels[0].SetActive(true);
    }

    public void FireSelect(int index)
    {
        audioSource.PlayOneShot(selectSkinSound);
        fireSelecteds[Settings.instance.meTowerSkins[0]].SetActive(false);
        Settings.instance.meTowerSkins[0] = index;
        fireSelecteds[Settings.instance.meTowerSkins[0]].SetActive(true);

        PlayerPrefs.SetInt("FireTower", index);
    }

    public void WaterSelect(int index)
    {
        audioSource.PlayOneShot(selectSkinSound);
        waterSelecteds[Settings.instance.meTowerSkins[1]].SetActive(false);
        Settings.instance.meTowerSkins[1] = index;
        waterSelecteds[Settings.instance.meTowerSkins[1]].SetActive(true);

        PlayerPrefs.SetInt("WaterTower", index);
    }

    public void MoonSelect(int index)
    {
        audioSource.PlayOneShot(selectSkinSound);
        moonSelecteds[Settings.instance.meTowerSkins[3]].SetActive(false);
        Settings.instance.meTowerSkins[3] = index;
        moonSelecteds[Settings.instance.meTowerSkins[3]].SetActive(true);

        PlayerPrefs.SetInt("MoonTower", index);
    }
}
