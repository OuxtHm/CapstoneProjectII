using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    public static SkillUI instance;
    DataManager dm;
    public ChangeSkill change;
    public SkillControler readySkill;
    public SkillControler nowSkill;
    public SkillControler nowUlt;
    public Transform ult;
    public Image ultSkillCoolTime;      // �ñر� ��ų ��Ÿ��
    public Image basicSkillCoolTime;    // �⺻ ��ų ��Ÿ��
    public bool useUlt;
    public bool useBasic;
    private void Awake()
    {
        instance = this;
        ultSkillCoolTime = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        basicSkillCoolTime = transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Image>();
        ult = transform.GetChild(0).GetComponent<Transform>();
        useUlt = false;
        useBasic = false;
    }
    private void Start()
    {
        dm = DataManager.instance;
        change = ChangeSkill.instance;
        GetSkillComponent();
        GetUltComponent();
    }
    void Update()
    {
        UseSkill();
    }

    void UseSkill()
    {
        if (Input.GetKeyDown(KeyCode.D) && !useUlt)
        {
            StartCoroutine(SkillUse(nowUlt.coolTime, ultSkillCoolTime, useUlt));
        }
        if (Input.GetKeyDown(KeyCode.E) && !useBasic)
        {
            StartCoroutine(SkillUse(nowSkill.coolTime, basicSkillCoolTime, useBasic));
        }
    }

    public IEnumerator SkillUse(float duration, Image img, bool useSkill)
    {
        useSkill = true;
        float elapsedTime = 0f; // ��� �ð�
        img.fillAmount = 1;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            img.fillAmount = Mathf.Lerp(1f, 0f, t);
            yield return null;
        }

        img.fillAmount = 0f;
        useSkill = false;
    }

    public void GetSkillComponent()
    {
        readySkill = change.readyskill.GetChild(0).GetComponent<SkillControler>();
        nowSkill = change.nowskill.GetChild(0).GetComponent<SkillControler>();

        // ������ �Ŵ����� ��ų ����
        dm.skillData.readySkill = readySkill.num;
        dm.skillData.nowSkill = nowSkill.num;
    }
    public void GetUltComponent()
    {
        nowUlt = transform.GetChild(0).GetComponentInChildren<SkillControler>();

        // ������ �Ŵ����� ��ų ����
        dm.skillData.ultSkill = nowUlt.num;
    }
}
