using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    public Image basicSkillCoolTime;    // 기본 스킬 쿨타임
    public Image ultSkillCoolTime;      // 궁극기 스킬 쿨타임
    public bool useBasic;
    public bool useUlt;
    private void Awake()
    {
        basicSkillCoolTime = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        ultSkillCoolTime = transform.GetChild(1).GetChild(1).GetComponent<Image>();
        useBasic = false;
        useUlt = false;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UseSkill();
    }

    void UseSkill()
    {
        if (Input.GetKeyDown(KeyCode.S) && !useBasic)
        {
            StartCoroutine(BasicSkillUse());
        }

        if (Input.GetKeyDown(KeyCode.D) && !useUlt)
        {
            StartCoroutine(UltSkillUse());
        }
    }

    public IEnumerator BasicSkillUse()
    {
        useBasic = true;
        float duration = 3f; // 보간에 걸리는 시간 (초)
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

    public IEnumerator UltSkillUse()
    {
        useUlt = true;
        float duration = 10f; // 보간에 걸리는 시간 (초)
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
}
