using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RandomSkillShop : MonoBehaviour
{
    public static RandomSkillShop instance;
    public SkillControler[] skillCon = new SkillControler[4];       // ��ų ���� ��ũ��Ʈ
    public RectTransform[] skillRect = new RectTransform[4];        
    public TextMeshProUGUI[] skillTxt = new TextMeshProUGUI[4];     // ��ų ������ �ۼ��� ����
    public TextMeshProUGUI[] priceTxt = new TextMeshProUGUI[4];     // ��ų ������ �ۼ��� ����
    private string ultSkillPath = "Prefabs/SkillIcon/Ult";
    private string commonSkillPath = "Prefabs/SkillIcon/Common";

    private void Awake()
    {
        instance = this;
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
        for (int i = 0; i < skillCon.Length; i++)
        {
            skillCon[i] = transform.GetChild(i).GetComponentInChildren<SkillControler>();
        }
        GetSkillData();
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
            RectTransform commonRect = selectCommon.GetComponent<RectTransform>();
            commonRect.sizeDelta = new Vector2(100, 100);
        }
        selectedIndexes.Clear();
    }

    public void GetSkillData()      // ��ų�� ������ �������� �Լ�
    {
        for(int i = 0; i < skillCon.Length; i++)
        {
            skillTxt[i].text = skillCon[i].description;
            priceTxt[i].text = skillCon[i].price.ToString();
        }
    }
}
