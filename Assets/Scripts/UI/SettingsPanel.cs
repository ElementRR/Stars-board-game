using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : TutPanel
{
    public Slider slider;
    public Toggle toggle;

    public void Start()
    {
        //Adds a listener to the main slider and invokes a method when the value changes.
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        toggle.onValueChanged.AddListener(delegate { ToggleChangeCheck(); });
    }
    private void Awake()
    {
        PlayerPrefs.GetFloat("musicVolume", Settings.musicVolume);
        
        slider.value = Settings.musicVolume;
        toggle.isOn = Settings.isFirstTimePlaying;
    }

    // Invoked when the value of the slider changes.
    public void ValueChangeCheck()
    {
        Settings.musicVolume = slider.value;
        Settings.instance.UpdateVolume();
    }
    private void ToggleChangeCheck()
    {
        Settings.isFirstTimePlaying = toggle.isOn;
        if(Settings.isFirstTimePlaying == false ) { PlayerPrefs.SetString("First time playing?","false"); } else
        {
            PlayerPrefs.SetString("First time playing?", "false");
        }
    }

}
