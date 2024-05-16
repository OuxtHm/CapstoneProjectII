using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageUI : MonoBehaviour
{
    public static StageUI instance;
    StageManager stageManager;
    public TextMeshProUGUI stageText;
    public void Awake()
    {
        instance = this;
        stageText = GetComponent<TextMeshProUGUI>();
    }
    public void Start()
    {
        stageManager = StageManager.instance;
        PrintStage(1,1);
    }
    public void PrintStage(int _nowStage,int _nowStageLv)
    {
       
        stageText.text = _nowStage.ToString() + "-" + _nowStageLv.ToString();

    }
}

