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
        PrintStage(1, 1);
    }

    public void PrintStage(int _nowStage, int _nowStageLv)
    {
        // _nowStage가 1, 2, 3 중 하나이고, _nowStageLv에 따라 다르게 표시
        if (_nowStage >= 1 && _nowStage <= 3)
        {
            switch (_nowStageLv)
            {
                case 4:
                    stageText.text = "상점";
                    break;
                case 5:
                    stageText.text = "Boss";
                    break;
                default:
                    stageText.text = _nowStage.ToString() + "-" + _nowStageLv.ToString();
                    break;
            }
        }
        else
        {
            // 다른 스테이지에 대한 처리; 필요에 따라 수정하거나 확장하세요.
            stageText.text = _nowStage.ToString() + "-" + _nowStageLv.ToString();
        }
    }
}
