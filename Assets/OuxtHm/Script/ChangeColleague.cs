using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColleague : MonoBehaviour
{
    public Image changeCoolTime;

    public Image colleague_1;  // ���� ������Ʈ
    public Image colleague_2;  // ���� ������Ʈ

    public Transform nowColleague;     // ��� ���� ĳ����
    public Transform readyColleague;   // ��� ���� ĳ����

    public bool change;     // ĳ���� ���� �ߴ��� Ȯ���ϴ� ����
    private void Awake()
    {
        changeCoolTime = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        change = false;   
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !change)
        {
            SwapColleague();
            StartCoroutine(ChangeColleagueCoolTime());
        }
    }

    public IEnumerator ChangeColleagueCoolTime()      // ���� ���� ��Ÿ�� ����
    {
        change = true;
        float duration = 7f; // ������ �ɸ��� �ð� (��)
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

    public void SwapColleague()     // ���� �̹��� ��ġ ��ȯ
    {
        nowColleague = transform.GetChild(0).GetComponent<Transform>();     // ���� ������� ĳ���� Transform ����
        readyColleague = transform.GetChild(1).GetComponent<Transform>();   // ������� ĳ���� Transform ����

        colleague_1 = nowColleague.GetChild(0).GetComponent<Image>();       // ������� ĳ���� Image ����
        colleague_2 = readyColleague.GetChild(0).GetComponent<Image>();     // ������� ĳ���� Transform ����

        colleague_1.transform.SetParent(readyColleague.transform, false);   // ������� ĳ���͸� ����� �������� �̵�
        colleague_1.transform.SetAsFirstSibling();                          // �θ� ������Ʈ�� ù��° �ڽ����� ����

        colleague_2.transform.SetParent(nowColleague.transform, false);     // ������� ĳ���͸� ����� �������� �̵�
        colleague_2.transform.SetAsFirstSibling();                          // �θ� ������Ʈ�� ù��° �ڽ����� ����
    }   
}
