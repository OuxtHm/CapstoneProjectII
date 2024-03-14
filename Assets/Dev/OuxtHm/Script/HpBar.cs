using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public Image hpBar;
    public TextMeshProUGUI hpText;

    public float maxHp;   // �ִ� ü��
    public float curHp;   // ���� ü��
    public float hpRatio;       // ü�� ����
    public float testCurHp;
    private void Awake()
    {
        hpBar = GetComponent<Image>();
        hpText = GetComponentInChildren<TextMeshProUGUI>();
        maxHp = 100;
        curHp = 100;
    }
    void Start()
    {
        ShowHpText();
        hpRatio = curHp / maxHp;
    }

    void ShowHpText()       // ü�� ��Ȳ�� �ؽ�Ʈ�� ǥ��
    {
        hpText.text = curHp.ToString() + " / " + maxHp.ToString();
    }

    void BringHp()      // �÷��̾�κ��� ü�� ������ �޾ƿ��� �Լ�
    {
        /*maxHp = player.playerMaxHp;
        curHp = player.playerCurHp;*/
    }

    public IEnumerator HandleHp()
    {
        while (Mathf.Abs(hpRatio - (curHp / maxHp)) >= 0.01f)
        {
            hpRatio = Mathf.Lerp(hpRatio, curHp / maxHp, Time.deltaTime * 2);
            hpBar.fillAmount = hpRatio;
            yield return null;
        }
        hpRatio = curHp / maxHp;
        hpBar.fillAmount = hpRatio;
    }

    public void Test()      // ���� �ڷ�ƾ�� �����ϱ� ���� ��ư�� �Լ�, ���� ������ �� ���� � Ȱ�밡��
    {
        curHp = testCurHp;
        ShowHpText();
        StartCoroutine(HandleHp());
    }
}
