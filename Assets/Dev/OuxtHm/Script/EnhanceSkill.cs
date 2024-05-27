using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnhanceSkill : MonoBehaviour
{
    SoundManager sm;
    Player player;
    ChangeSkill changeSkill;
    public SkillControler[] skill = new SkillControler[2];      // �÷��̾��� ��ų
    public RectTransform[,] rect = new RectTransform[2,2];      // ��ų �̹����� ��� RectTransform
    public TextMeshProUGUI[,] description = new TextMeshProUGUI[2,2];     // ��ȭ ����
    public Button[] enhanceBtn = new Button[2];     // ��ų ��ȭ ��ư
    private GameObject warning;     // ��� ���� ���â
    private int[] cost = new int[2];
    private void Awake()
    {
        cost = new int[] { 100, 300 };
        warning = Resources.Load<GameObject>("Prefabs/Warning_canvas");
        for (int i = 0; i < 2; i++)
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
        sm = SoundManager.instance;
        player = Player.instance;
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
                if(player.money > 0)
                {
                    Debug.Log(cost[skill[index].level - 1]);
                    player.money -= cost[skill[index].level - 1];
                    skill[index].level++;
                    skill[index].coolTime *= 0.9f;
                    skill[index].coefficient *= 1.2f;
                    SkillTextUpdate(index);
                    if (skill[index].level == 3)
                    {
                        Destroy(enhanceBtn[index].gameObject);
                    }
                }
                else
                {
                    Instantiate(warning);
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
        // ���� ���� ��ų �ɷ�ġ ���
        description[index, 0].text = "Level" + skill[index].level.ToString() + "\n" 
            + "��Ÿ��" + skill[index].coolTime.ToString("F2") + "\n" 
            + "��ų ���" + skill[index].coefficient.ToString("F1");

        if (skill[index].level == 2)
        {
            description[index, 1].text = "MAX";     // ��ȭ �� ��� ���� ���
        }
        if (skill[index].level == 3)
        {
            // ��,���� ��ȭ�� ��� ���
            description[index, 0].text = "MAX";
            description[index, 1].text = "MAX";
        }
        else
        {
            int plusLevel = skill[index].level + 1;
            float enhanceCoolTime = skill[index].coolTime * 0.9f;
            float enhanceCoefficent = skill[index].coefficient * 1.2f;
            description[index, 1].text = "Level" + plusLevel.ToString() + "\n" 
                + "��Ÿ��" + enhanceCoolTime.ToString("F2") + "\n" 
                + "��ų ���" + enhanceCoefficent.ToString("F1");
        }
    }
}




