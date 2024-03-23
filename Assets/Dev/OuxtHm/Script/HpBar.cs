using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public Player player;
    public Image hpBar;
    public TextMeshProUGUI hpText;

    public float hpRatio;       // ü�� ����
    public float testCurHp;
    private void Awake()
    {
        hpBar = GetComponent<Image>();
        hpText = GetComponentInChildren<TextMeshProUGUI>();
    }
    void Start()
    {
        player = Player.instance;
        ShowHpText();
        hpRatio = player.curHp / player.maxHp;
    }

    void ShowHpText()       // ü�� ��Ȳ�� �ؽ�Ʈ�� ǥ��
    {
        hpText.text = player.curHp.ToString() + " / " + player.maxHp.ToString();
    }

    public IEnumerator HandleHp()
    {
        while (Mathf.Abs(hpRatio - (player.curHp / player.maxHp)) >= 0.01f)
        {
            hpRatio = Mathf.Lerp(hpRatio, player.curHp / player.maxHp, Time.deltaTime * 2);
            hpBar.fillAmount = hpRatio;
            yield return null;
        }
        hpRatio = player.curHp / player.maxHp;
        hpBar.fillAmount = hpRatio;
    }

    public void ChangeHp()      // ���� �ڷ�ƾ�� �����ϱ� ���� ��ư�� �Լ�, ���� ������ �� ���� � Ȱ�밡��
    {
        player.curHp = testCurHp;
        ShowHpText();
        StartCoroutine(HandleHp());
    }
}
