using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    public static SkillUI instance;
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

    void Update()
    {
        UseSkill();
    }

    void UseSkill()
    {
        if (Input.GetKeyDown(KeyCode.D) && !useUlt)
        {
            StartCoroutine(UltSkillUse(10f));
        }
        if(Input.GetKeyDown(KeyCode.E) && !useBasic)
        {
            StartCoroutine(BasicSkillUse(5f));
        }
    }

    public IEnumerator UltSkillUse(float duration)
    {
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

}
