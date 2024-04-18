using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundOption : MonoBehaviour
{
    Slider masterSlider;
    Slider bgmSlider;
    Slider sfxSlider;
    TextMeshProUGUI masterTxt;
    TextMeshProUGUI bgmTxt;
    TextMeshProUGUI sfxTxt;

    private void Awake()
    {
        masterSlider = transform.GetChild(1).GetChild(0).GetComponent<Slider>();
        bgmSlider = transform.GetChild(1).GetChild(1).GetComponent<Slider>();
        sfxSlider = transform.GetChild(1).GetChild(2).GetComponent<Slider>();

        masterTxt = transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        bgmTxt = transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
        sfxTxt = transform.GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    private void LateUpdate()
    {
        SliderValueText();
    }
    public void SliderValueText()
    {
        float masterVolume = masterSlider.value * 100;
        float bgmVolume = bgmSlider.value * 100;
        float sfxVolume = sfxSlider.value * 100;
        masterTxt.text = Mathf.FloorToInt(masterVolume) + "%".ToString();
        bgmTxt.text = Mathf.FloorToInt(bgmVolume) + "%".ToString(); 
        sfxTxt.text = Mathf.FloorToInt(sfxVolume) + "%".ToString();
    }

}
