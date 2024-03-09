using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public Image hpBar;
    public TextMeshProUGUI hpText;

    public float maxHp;   // 최대 체력
    public float curHp;   // 현재 체력
    public float hpRatio;       // 체력 비율

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

    void HandleHp()     // 체력바 애니메이션
    {
        hpRatio = Mathf.Lerp(hpRatio, curHp / maxHp, Time.deltaTime * 2);
        hpBar.fillAmount = hpRatio;
    }

    void ShowHpText()       // 체력 상황을 텍스트로 표현
    {
        hpText.text = curHp.ToString() + " / " + maxHp.ToString();
    }

    void BringHp()      // 플레이어로부터 체력 변수를 받아오는 함수
    {
        /*maxHp = player.playerMaxHp;
        curHp = player.playerCurHp;*/
    }
}
