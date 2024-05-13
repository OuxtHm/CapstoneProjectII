using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSkill : MonoBehaviour
{
    public static ChangeSkill instance;
    public Image changeCoolTime;

    public Image skill_1;  // ��ų ������Ʈ
    public Image skill_2;  // ��ų ������Ʈ

    public Transform nowskill;     // ��� ���� ��ų
    public Transform readyskill;   // ��� ���� ��ų

    public bool change;     // ��ų ���� �ߴ��� Ȯ���ϴ� ����
    private void Awake()
    {
        instance = this;
        changeCoolTime = transform.GetChild(0).GetComponent<Image>();
        nowskill = transform.GetChild(1).GetComponent<Transform>();     // ���� ������� ��ų Transform ����
        readyskill = transform.GetChild(2).GetComponent<Transform>();   // ������� ��ų Transform ����
        skill_1 = nowskill.GetChild(0).GetComponent<Image>();       // ������� ��ų Image ����
        skill_2 = readyskill.GetChild(0).GetComponent<Image>();     // ������� ��ų Image ����
        change = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !change)
        {
            Swapskill();
            StartCoroutine(ChangeskillCoolTime(7f));
        }
    }

    public IEnumerator ChangeskillCoolTime(float duration)      // �⺻ ��ų ���� ��Ÿ�� ����
    {
        change = true;
        float elapsedTime = 0f; // ��� �ð�
        changeCoolTime.fillAmount = 1;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            changeCoolTime.fillAmount = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }

        changeCoolTime.fillAmount = 1f;
        change = false;
    }

    public void Swapskill()     // �⺻ ��ų �̹��� ��ġ ��ȯ
    {
        nowskill = transform.GetChild(1).GetComponent<Transform>();     // ���� ������� ��ų Transform ����
        readyskill = transform.GetChild(2).GetComponent<Transform>();   // ������� ��ų Transform ����

        skill_1 = nowskill.GetChild(0).GetComponent<Image>();       // ������� ��ų Image ����
        skill_2 = readyskill.GetChild(0).GetComponent<Image>();     // ������� ��ų Image ����

        skill_1.transform.SetParent(readyskill.transform, false);   // ������� ��ų ����� �������� �̵�
        skill_1.transform.SetAsFirstSibling();                          // �θ� ������Ʈ�� ù��° �ڽ����� ����

        skill_2.transform.SetParent(nowskill.transform, false);     // ������� ��ų ����� �������� �̵�
        skill_2.transform.SetAsFirstSibling();                          // �θ� ������Ʈ�� ù��° �ڽ����� ����
    }
}
