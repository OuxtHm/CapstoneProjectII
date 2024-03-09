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

    private void Awake()
    {
        hpBar = GetComponent<Image>();
        hpText = GetComponentInChildren<TextMeshProUGUI>();
        maxHp = 100;
        curHp = 100;
    }
    void Start()
    {
        hpRatio = curHp / maxHp;
    }

    private void LateUpdate()
    {
        ShowHpText();
        BringHp();
        HandleHp();
    }

    void HandleHp()     // ü�¹� �ִϸ��̼�
    {
        hpRatio = Mathf.Lerp(hpRatio, curHp / maxHp, Time.deltaTime * 2);
        hpBar.fillAmount = hpRatio;
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
}
