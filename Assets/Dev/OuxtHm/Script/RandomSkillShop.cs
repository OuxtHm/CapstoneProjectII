using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RandomSkillShop : MonoBehaviour
{
    public RectTransform[] skillRect = new RectTransform[4];
    public TextMeshProUGUI[] skillTxt = new TextMeshProUGUI[4];
    private string ultSkillPath = "Prefabs/SkillIcon/Ult";
    private string commonSkillPath = "Prefabs/SkillIcon/Common";
    string ult_Lightning;
    string ult_Beam;
    string common_Arrow;
    string common_Slash;
    string common_AtkBuff;
    string common_Blood;
    string common_Reach;
    private void Awake()
    {
        SkillText();
        for (int i = 0; i < skillRect.Length; i++)
        {
            skillRect[i] = transform.GetChild(i).GetComponent<RectTransform>();
            skillTxt[i] = skillRect[i].transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    private void Start()
    {
        ShopSaleSkillCreate();
    }
    public void ShopSaleSkillCreate()     // 상점에 판매할 스킬 생성
    {
        RandomUlt();
        RandomCommon();
    }

    private void RandomUlt()        // 랜덤 궁극기 생성
    {
        Object[] ult = Resources.LoadAll(ultSkillPath, typeof(GameObject));
        int randomIndex = Random.Range(0, ult.Length);
        GameObject selectUlt = (GameObject)ult[randomIndex];
        Instantiate(selectUlt, skillRect[0]);

        ConnectSkillText(selectUlt.name, skillTxt[0]);

        RectTransform ultRect = selectUlt.GetComponent<RectTransform>();
        ultRect.sizeDelta = new Vector2(100, 100);
    }

    private void RandomCommon()     // 랜덤 일반 스킬 생성
    {
        Object[] common = Resources.LoadAll(commonSkillPath, typeof(GameObject));
        List<int> selectedIndexes = new List<int>(); // 이미 선택된 인덱스를 저장할 리스트

        for (int i = 1; i < 4; i++)
        {
            int randomIndex = Random.Range(0, common.Length);

            // 이미 선택된 인덱스면 다시 추출
            while (selectedIndexes.Contains(randomIndex))
            {
                randomIndex = Random.Range(0, common.Length);
            }

            // 선택된 인덱스를 리스트에 추가
            selectedIndexes.Add(randomIndex);

            GameObject selectCommon = (GameObject)common[randomIndex];
            Instantiate(selectCommon, skillRect[i]);
            ConnectSkillText(selectCommon.name, skillTxt[i]);

            RectTransform commonRect = selectCommon.GetComponent<RectTransform>();
            commonRect.sizeDelta = new Vector2(100, 100);
        }

    }

    private void SkillText()        // 스킬 설명 생성
    {
        ult_Lightning = "전방으로 번개를 세 번 내려칩니다.";
        ult_Beam = "전방으로 빛의 기둥을 소환합니다.";
        common_Arrow = "검을 휘두르며 빛의 화살을 생성합니다.";
        common_Slash = "전방으로 검기를 날립니다.";
        common_AtkBuff = "공격력을 잠시 소폭 상승시킵니다.";
        common_Blood = "피로 물들입니다.";
        common_Reach = "사거리가 상승된 검기를 생성합니다.";
    }

    private void ConnectSkillText(string skillName, TextMeshProUGUI skillTextMeshProUGUI)       // 스킬 이름에 따른 스킬 설명 출력 함수
    {
        switch (skillName)
        {
            case "Lightning_icon": skillTextMeshProUGUI.text = ult_Lightning; break;
            case "Beam_icon": skillTextMeshProUGUI.text = ult_Beam; break;
            case "Slash_icon": skillTextMeshProUGUI.text = common_Slash; break;
            case "Arrow_icon": skillTextMeshProUGUI.text = common_Arrow; break;
            case "AtkBuff_icon": skillTextMeshProUGUI.text = common_AtkBuff; break;
            case "Blood_icon": skillTextMeshProUGUI.text = common_Blood; break;
            case "Reach_icon": skillTextMeshProUGUI.text = common_Reach; break;
        }
    }
}
