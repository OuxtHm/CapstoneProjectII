using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    public Image basicSkillCoolTime;    // �⺻ ��ų ��Ÿ��
    public Image ultSkillCoolTime;      // �ñر� ��ų ��Ÿ��
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
        float duration = 3f; // ������ �ɸ��� �ð� (��)
        float elapsedTime = 0f; // ��� �ð�
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
        float duration = 10f; // ������ �ɸ��� �ð� (��)
        float elapsedTime = 0f; // ��� �ð�
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
