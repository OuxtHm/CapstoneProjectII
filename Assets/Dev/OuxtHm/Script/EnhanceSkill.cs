using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnhanceSkill : MonoBehaviour
{
    ChangeSkill changeSkill;
    public SkillControler[] skill = new SkillControler[2];      // �÷��̾��� ��ų
    public RectTransform[,] rect = new RectTransform[2,2];      // ��ų �̹����� ��� RectTransform
    public TextMeshProUGUI[,] description = new TextMeshProUGUI[2,2];     // ��ȭ ����
    public Button[] enhanceBtn = new Button[2];     // ��ų ��ȭ ��ư
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
        changeSkill = ChangeSkill.instance;

        GameObject[] skill_1 = new GameObject[2];
        GameObject[] skill_2 = new GameObject[2];
        RectTransform[] skillRect1 = new RectTransform[2];
        RectTransform[] skillRect2 = new RectTransform[2];

        for (int i = 0; i < 2; i++)
        {
            skill_1[i] = InstantiateSkill(changeSkill.skill_1.gameObject, i, 0, skillRect1);
            skill_2[i] = InstantiateSkill(changeSkill.skill_2.gameObject, i, 1, skillRect2);
        }
        skill[0] = changeSkill.skill_1.GetComponent<SkillControler>();
        skill[1] = changeSkill.skill_2.GetComponent<SkillControler>();

        for (int i = 0; i < 2; i++)
        {
            enhanceBtn[i] = transform.GetChild(i).GetChild(0).GetComponent<Button>();
            SkillTextUpdate(i);

            int index = i;

            // ��ȭ ��ư ���
            enhanceBtn[i].onClick.AddListener(() =>
            {
                skill[index].level++;
                SkillTextUpdate(index);
                if (skill[index].level == 3)
                {
                    Destroy(enhanceBtn[index].gameObject);
                }
            });
        }

    }

    private GameObject InstantiateSkill(GameObject skillPrefab, int index, int rectIndex, RectTransform[] skillRects)   // ��ȭ â�� ���� ������ �ִ� ��ų ������ ����
    {
        GameObject skillInstance = Instantiate(skillPrefab.transform.gameObject, rect[rectIndex, index]); 
        RectTransform skillRect = skillInstance.GetComponent<RectTransform>(); 

        skillRect.anchoredPosition = new Vector2(0, 60);
        skillRect.sizeDelta = new Vector2(130, 130);

        skillRects[index] = skillRect; 

        return skillInstance;
    }

    public void SkillTextUpdate(int index)  // ��ȭ â�� �ؽ�Ʈ ���� ������Ʈ
    {
        description[index, 0].text = "Level" + skill[index].level.ToString();

        if (skill[index].level == 2)
        {
            description[index, 1].text = "MAX";
        }
        if (skill[index].level == 3)
        {
            description[index, 0].text = "MAX";
            description[index, 1].text = "MAX";
        }
        else
        {
            int plusLevel = skill[index].level + 1;
            description[index, 1].text = "Level" + plusLevel.ToString();
        }
    }
}




