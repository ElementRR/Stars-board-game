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

    private void Awake()
    {
        fireSelecteds[Settings.instance.meTowerSkins[0]].SetActive(true);
        waterSelecteds[Settings.instance.meTowerSkins[1]].SetActive(true);
        moonSelecteds[Settings.instance.meTowerSkins[3]].SetActive(true);
    }

    public void GoToPanel(int index)
    {
        shopPanels[0].SetActive(false);
        shopPanels[index].SetActive(true);
    }
    public void BackToMain(int index)
    {
        shopPanels[index].SetActive(false);
        shopPanels[0].SetActive(true);
    }

    public void FireSelect(int index)
    {
        fireSelecteds[Settings.instance.meTowerSkins[0]].SetActive(false);
        Settings.instance.meTowerSkins[0] = index;
        fireSelecteds[Settings.instance.meTowerSkins[0]].SetActive(true);
    }

    public void WaterSelect(int index)
    {
        waterSelecteds[Settings.instance.meTowerSkins[1]].SetActive(false);
        Settings.instance.meTowerSkins[1] = index;
        waterSelecteds[Settings.instance.meTowerSkins[1]].SetActive(true);
    }

    public void MoonSelect(int index)
    {
        moonSelecteds[Settings.instance.meTowerSkins[3]].SetActive(false);
        Settings.instance.meTowerSkins[3] = index;
        moonSelecteds[Settings.instance.meTowerSkins[3]].SetActive(true);
    }
}
