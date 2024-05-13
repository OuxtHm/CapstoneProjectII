using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    public static SkillUI instance;
    public ChangeSkill change;
    public SkillControler nowSkill;
    public SkillControler nowUlt;
    public Image ultSkillCoolTime;      // 궁극기 스킬 쿨타임
    public Image basicSkillCoolTime;    // 기본 스킬 쿨타임
    public bool useUlt;
    public bool useBasic;
    private void Awake()
    {
        instance = this;
        ultSkillCoolTime = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        basicSkillCoolTime = transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Image>();
        useUlt = false;
        useBasic = false;
    }
    private void Start()
    {
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
            StartCoroutine(UltSkillUse(nowUlt.coolTime));
        }
        if (Input.GetKeyDown(KeyCode.E) && !useBasic)
        {
            StartCoroutine(BasicSkillUse(nowSkill.coolTime));
        }
    }

    public IEnumerator UltSkillUse(float duration)
    {
        Debug.Log(duration);
        useUlt = true;
        float elapsedTime = 0f; // 경과 시간
        ultSkillCoolTime.fillAmount = 1;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            ultSkillCoolTime.fillAmount = Mathf.Lerp(1f, 0f, t);
            yield return null;
        }

        ultSkillCoolTime.fillAmount = 0f;
        useUlt = false;
    }

    public IEnumerator BasicSkillUse(float duration)
    {
        useBasic = true;
        float elapsedTime = 0f; // 경과 시간
        basicSkillCoolTime.fillAmount = 1;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            basicSkillCoolTime.fillAmount = Mathf.Lerp(1f, 0f, t);
            yield return null;
        }

        basicSkillCoolTime.fillAmount = 0f;
        useBasic = false;
    }
    public void GetSkillComponent()
    {
        nowSkill = change.nowskill.GetChild(0).GetComponent<SkillControler>();
    }
    public void GetUltComponent()
    {
        nowUlt = transform.GetChild(0).GetComponentInChildren<SkillControler>();
    }
}
