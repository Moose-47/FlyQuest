using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : BaseMenu
{
    public Button backBtn;

    public Slider volSlider;
    public TMP_Text volSliderTxt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Init(MenuController context)
    {
        base.Init(context);
        state = MenuStates.Settings;

        if (backBtn) backBtn.onClick.AddListener(JumpBack);

        if (volSlider)
        {
            volSlider.onValueChanged.AddListener(OnSliderValueChanged);
            OnSliderValueChanged(volSlider.value);
        }
    }

    void OnSliderValueChanged(float value)
    {
        float roundedValue = Mathf.Round(value * 100);
        if (volSliderTxt) volSliderTxt.text = $"{roundedValue}%";
    }

}
