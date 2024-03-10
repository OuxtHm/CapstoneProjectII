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

    void ShowHpText()       // 체력 상황을 텍스트로 표현
    {
        hpText.text = curHp.ToString() + " / " + maxHp.ToString();
    }

    void BringHp()      // 플레이어로부터 체력 변수를 받아오는 함수
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

    public void Test()      // 위의 코루틴을 실행하기 위한 버튼용 함수, 추후 데미지 및 포션 등에 활용가능
    {
        curHp = testCurHp;
        ShowHpText();
        StartCoroutine(HandleHp());
    }
}
