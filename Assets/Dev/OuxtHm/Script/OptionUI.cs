using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public Button[] btn = new Button[3];
    public GameObject btnArray;
    public GameObject soundsAray;
    public Button soundsBackBtn;

    private void Awake()
    {
        btnArray = transform.GetChild(0).GetChild(1).gameObject;
        soundsAray = transform.GetChild(0).GetChild(2).gameObject;
        for(int i = 0; i < btn.Length; i++)
        {
            btn[i] = transform.GetChild(0).GetChild(1).GetChild(i).GetComponent<Button>();
        }
        btn[2].onClick.AddListener(() =>
        {
            btnArray.SetActive(false);
            soundsAray.SetActive(true);
        });
        soundsBackBtn = transform.GetChild(0).GetChild(2).GetChild(3).GetComponent<Button>();
        soundsBackBtn.onClick.AddListener(() =>
        {
            btnArray.SetActive(true);
            soundsAray.SetActive(false);
        });
    }

}
