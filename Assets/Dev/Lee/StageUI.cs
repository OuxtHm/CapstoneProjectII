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
        // _nowStage�� 1, 2, 3 �� �ϳ��̰�, _nowStageLv�� ���� �ٸ��� ǥ��
        if (_nowStage >= 1 && _nowStage <= 3)
        {
            switch (_nowStageLv)
            {
                case 4:
                    stageText.text = "����";
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
            // �ٸ� ���������� ���� ó��; �ʿ信 ���� �����ϰų� Ȯ���ϼ���.
            stageText.text = _nowStage.ToString() + "-" + _nowStageLv.ToString();
        }
    }
}
