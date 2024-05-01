using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class EnhanceSkill : MonoBehaviour
{
    GameManager gm;
    public RectTransform[,] rect = new RectTransform[2,2];      // 스킬 이미지를 띄울 RectTransform
    public TextMeshProUGUI[,] description = new TextMeshProUGUI[2,2];     // 강화 내용

    private void Awake()
    {
        for(int i = 0; i < 2; i++)
        {
            for(int j = 0; j < 2; j++)
            {
                rect[i,j] = transform.GetChild(i).GetChild(j + 1).GetComponent<RectTransform>();
                description[i, j] = rect[i,j].GetChild(0).GetComponent<TextMeshProUGUI>();
            }
        }
    }

    private void Start()
    {
        gm = GameManager.instance;

        GameObject[] skill_1 = new GameObject[2];
        GameObject[] skill_2 = new GameObject[2];
        RectTransform[] skillRect1 = new RectTransform[2];
        RectTransform[] skillRect2 = new RectTransform[2];

        for (int i = 0; i < 2; i++)
        {
            skill_1[i] = InstantiateSkill(gm.changeSkill.skill_1.gameObject, i, 0, skillRect1);
            skill_2[i] = InstantiateSkill(gm.changeSkill.skill_2.gameObject, i, 1, skillRect2);
        }
    }

    // 스킬 인스턴스화 및 RectTransform 설정
    private GameObject InstantiateSkill(GameObject skillPrefab, int index, int rectIndex, RectTransform[] skillRects)
    {
        GameObject skillInstance = Instantiate(skillPrefab.transform.gameObject, rect[rectIndex, index]); 
        RectTransform skillRect = skillInstance.GetComponent<RectTransform>(); 

        skillRect.anchoredPosition = new Vector2(0, 60);
        skillRect.sizeDelta = new Vector2(130, 130);

        skillRects[index] = skillRect; 

        return skillInstance;
    }

}
