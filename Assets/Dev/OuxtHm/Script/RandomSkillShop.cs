using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RandomSkillShop : MonoBehaviour
{
    public static RandomSkillShop instance;
    public RectTransform[] skillRect = new RectTransform[4];
    public TextMeshProUGUI[] skillTxt = new TextMeshProUGUI[4];
    public TextMeshProUGUI[] priceTxt = new TextMeshProUGUI[4];
    public List<int> price = new List<int>();
    private string ultSkillPath = "Prefabs/SkillIcon/Ult";
    private string commonSkillPath = "Prefabs/SkillIcon/Common";
    string ult_Lightning;
    string ult_Beam;
    string common_Arrow;
    string common_Slash;
    string common_AtkBuff;
    string common_Blood;
    string common_Reach;
    int lightningPrice;
    int beamPrice;
    int arrowPrice;
    int slashPrice;
    int atkBuffPrice;
    int bloodPrice;
    int reachPrice;
    private void Awake()
    {
        instance = this;
        SkillText();
        for (int i = 0; i < skillRect.Length; i++)
        {
            skillRect[i] = transform.GetChild(i).GetComponent<RectTransform>();
            skillTxt[i] = skillRect[i].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
            priceTxt[i] = skillRect[i].transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();   
        }
    }

    private void Start()
    {
        ShopSaleSkillCreate();
    }
    public void ShopSaleSkillCreate()     // ������ �Ǹ��� ��ų ����
    {
        RandomUlt();
        RandomCommon();
    }

    private void RandomUlt()        // ���� �ñر� ����
    {
        Object[] ult = Resources.LoadAll(ultSkillPath, typeof(GameObject));
        int randomIndex = Random.Range(0, ult.Length);
        GameObject selectUlt = (GameObject)ult[randomIndex];
        Instantiate(selectUlt, skillRect[0]);

        ConnectSkillText(selectUlt.name, skillTxt[0], priceTxt[0]);

        RectTransform ultRect = selectUlt.GetComponent<RectTransform>();
        ultRect.sizeDelta = new Vector2(100, 100);
    }

    private void RandomCommon()     // ���� �Ϲ� ��ų ����
    {
        Object[] common = Resources.LoadAll(commonSkillPath, typeof(GameObject));
        List<int> selectedIndexes = new List<int>(); // �̹� ���õ� �ε����� ������ ����Ʈ

        for (int i = 1; i < 4; i++)
        {
            int randomIndex = Random.Range(0, common.Length);
            while (selectedIndexes.Contains(randomIndex))
            {
                randomIndex = Random.Range(0, common.Length);
            }
            selectedIndexes.Add(randomIndex);

            GameObject selectCommon = (GameObject)common[randomIndex];
            Instantiate(selectCommon, skillRect[i]);
            ConnectSkillText(selectCommon.name, skillTxt[i], priceTxt[i]);

            RectTransform commonRect = selectCommon.GetComponent<RectTransform>();
            commonRect.sizeDelta = new Vector2(100, 100);
        }
        selectedIndexes.Clear();
    }

    private void SkillText()        // ��ų ���� �� ���� ����
    {
        ult_Lightning = "�������� ������ �� �� ����Ĩ�ϴ�.";
        ult_Beam = "�������� ���� ����� ��ȯ�մϴ�.";
        common_Arrow = "���� �ֵθ��� ���� ȭ���� �����մϴ�.";
        common_Slash = "�������� �˱⸦ �����ϴ�.";
        common_AtkBuff = "���ݷ��� ��� ���� ��½�ŵ�ϴ�.";
        common_Blood = "�Ƿ� �����Դϴ�.";
        common_Reach = "��Ÿ��� ������ �˱⸦ �����մϴ�.";

        lightningPrice = 1000;
        beamPrice = 1000;
        arrowPrice = 200;
        slashPrice = 200;
        atkBuffPrice = 300;
        bloodPrice = 400;
        reachPrice = 400;
    }

    private void ConnectSkillText(string skillName, TextMeshProUGUI skillTextMeshProUGUI, TextMeshProUGUI priceTextMeshProUGUI)       // ��ų �̸��� ���� ��ų ���� ��� �Լ�
    {
        switch (skillName)
        {
            case "Lightning_icon": 
                skillTextMeshProUGUI.text = ult_Lightning; 
                priceTextMeshProUGUI.text = lightningPrice.ToString();
                price.Add(lightningPrice);
                break;
            case "Beam_icon": 
                skillTextMeshProUGUI.text = ult_Beam; 
                priceTextMeshProUGUI.text = beamPrice.ToString(); 
                price.Add(beamPrice);
                break;
            case "Arrow_icon": 
                skillTextMeshProUGUI.text = common_Arrow;
                priceTextMeshProUGUI.text = arrowPrice.ToString();
                price.Add(arrowPrice);
                break;
            case "Slash_icon":
                skillTextMeshProUGUI.text = common_Slash;
                priceTextMeshProUGUI.text = slashPrice.ToString();
                price.Add(slashPrice);
                break;
            case "AtkBuff_icon": 
                skillTextMeshProUGUI.text = common_AtkBuff;
                priceTextMeshProUGUI.text = atkBuffPrice.ToString();
                price.Add(atkBuffPrice);
                break;
            case "Blood_icon": 
                skillTextMeshProUGUI.text = common_Blood;
                priceTextMeshProUGUI.text = bloodPrice.ToString();
                price.Add(bloodPrice);
                break;
            case "Reach_icon": 
                skillTextMeshProUGUI.text = common_Reach;
                priceTextMeshProUGUI.text = reachPrice.ToString();
                price.Add(reachPrice);
                break;
        }
    }
}
