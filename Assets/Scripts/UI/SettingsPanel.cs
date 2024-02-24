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
    protected override void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        PlayerPrefs.GetFloat("musicVolume", AudioListener.volume);
        
        slider.value = AudioListener.volume;
        toggle.isOn = Settings.isFirstTimePlaying;
    }

    // Invoked when the value of the slider changes.
    public void ValueChangeCheck()
    {
        AudioListener.volume = slider.value;
        PlayerPrefs.SetFloat("musicVolume", AudioListener.volume);
    }
    private void ToggleChangeCheck()
    {
        Settings.isFirstTimePlaying = toggle.isOn;

        int boolToInt = toggle.isOn ? 1 : 0;
        PlayerPrefs.SetInt("isFirstTime", boolToInt);
    }
    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(delegate { ValueChangeCheck(); });
        toggle.onValueChanged.RemoveListener(delegate { ToggleChangeCheck(); });
    }
}
