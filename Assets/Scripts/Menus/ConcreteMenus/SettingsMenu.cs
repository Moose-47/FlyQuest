using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;



public class SettingsMenu : BaseMenu
{
    public AudioMixer mixer;

    public Button backBtn;

    public Slider masterSlider;
    public TMP_Text masterTxt;

    public Slider musicSlider;
    public TMP_Text musicTxt;

    public Slider sfxSlider;
    public TMP_Text sfxTxt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Init(MenuController context)
    {
        base.Init(context);
        state = MenuStates.Settings;

        if (backBtn) backBtn.onClick.AddListener(JumpBack);

        setupSliderInfo(masterSlider, masterTxt, "MasterVol");
        setupSliderInfo(musicSlider, musicTxt, "MusicVol");
        setupSliderInfo(sfxSlider, sfxTxt, "SFXVol");
    }

    void OnSliderValueChanged(float value, Slider slider, TMP_Text sliderText, string parameterName)
    {
        value = (value == 0.0f) ? -80.0f : 20.0f * Mathf.Log10(slider.value);
        sliderText.text = (value == -80.0f) ? "0%" : $"{(int)(slider.value*100)}%";
        mixer.SetFloat(parameterName, value);
    }
    void setupSliderInfo(Slider slider, TMP_Text sliderText, string parameterName)
    {
        slider.onValueChanged.AddListener((value) => OnSliderValueChanged(value, slider, sliderText, parameterName));
        OnSliderValueChanged(slider.value, slider, sliderText, parameterName);
    }
}
