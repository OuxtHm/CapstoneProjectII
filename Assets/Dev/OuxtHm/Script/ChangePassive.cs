using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangePassive : MonoBehaviour
{
    public Image changeCoolTime;

    public Image passive_1;  // �нú� ������Ʈ
    public Image passive_2;  // �нú� ������Ʈ

    public Transform nowPassive;     // ��� ���� �нú�
    public Transform readyPassive;   // ��� ���� �нú�

    public bool change;     // �нú� ���� �ߴ��� Ȯ���ϴ� ����
    private void Awake()
    {
        changeCoolTime = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        change = false;   
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !change)
        {
            SwapPassive();
            StartCoroutine(ChangePassiveCoolTime(7f));
        }
    }

    public IEnumerator ChangePassiveCoolTime(float duration)      // �нú� ���� ��Ÿ�� ����
    {
        change = true;
        float elapsedTime = 0f; // ��� �ð�
        changeCoolTime.fillAmount = 1;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            changeCoolTime.fillAmount = Mathf.Lerp(1f, 0f, t);
            yield return null;
        }

        changeCoolTime.fillAmount = 0f;
        change = false;
    }

    public void SwapPassive()     // �нú� �̹��� ��ġ ��ȯ
    {
        nowPassive = transform.GetChild(0).GetComponent<Transform>();     // ���� ������� �нú� Transform ����
        readyPassive = transform.GetChild(1).GetComponent<Transform>();   // ������� �нú� Transform ����

        passive_1 = nowPassive.GetChild(0).GetComponent<Image>();       // ������� �нú� Image ����
        passive_2 = readyPassive.GetChild(0).GetComponent<Image>();     // ������� �нú� Transform ����

        passive_1.transform.SetParent(readyPassive.transform, false);   // ������� �нú� ����� �������� �̵�
        passive_1.transform.SetAsFirstSibling();                          // �θ� ������Ʈ�� ù��° �ڽ����� ����

        passive_2.transform.SetParent(nowPassive.transform, false);     // ������� �нú� ����� �������� �̵�
        passive_2.transform.SetAsFirstSibling();                          // �θ� ������Ʈ�� ù��° �ڽ����� ����
    }   
}