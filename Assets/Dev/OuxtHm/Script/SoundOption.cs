using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundOption : MonoBehaviour
{
    public static SoundOption instance;
    DataManager dm;
    SoundManager sm;
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;
    TextMeshProUGUI masterTxt;
    TextMeshProUGUI bgmTxt;
    TextMeshProUGUI sfxTxt;
    Button saveBtn;
    public AudioClip clickSounds;      // 버튼 클릭 사운드

    private void Awake()
    {
        instance = this;
        masterSlider = transform.GetChild(1).GetChild(0).GetComponent<Slider>();
        bgmSlider = transform.GetChild(1).GetChild(1).GetComponent<Slider>();
        sfxSlider = transform.GetChild(1).GetChild(2).GetComponent<Slider>();

        masterTxt = transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        bgmTxt = transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
        sfxTxt = transform.GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>();

        saveBtn = transform.GetChild(4).GetComponent<Button>();
    }

    private void Start()
    {
        dm = DataManager.instance;
        sm = SoundManager.instance;
        masterSlider.value = dm.optionData.masterValue;
        bgmSlider.value = dm.optionData.bgmValue;
        sfxSlider.value = dm.optionData.sfxValue;

        saveBtn.onClick.AddListener(() =>
        {
            sm.SFXPlay(clickSounds);
            WriteSoundValue();
            dm.SaveOptionData();
        });
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
    public void WriteSoundValue()
    {
        dm.optionData.masterValue = masterSlider.value;
        dm.optionData.bgmValue = bgmSlider.value;
        dm.optionData.sfxValue = sfxSlider.value;
    }
}
