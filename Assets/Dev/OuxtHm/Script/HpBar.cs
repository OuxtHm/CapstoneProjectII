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

    public float hpRatio;       // 체력 비율
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

    void ShowHpText()       // 체력 상황을 텍스트로 표현
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

    public void ChangeHp()      // 위의 코루틴을 실행하기 위한 버튼용 함수, 추후 데미지 및 포션 등에 활용가능
    {
        player.curHp = testCurHp;
        ShowHpText();
        StartCoroutine(HandleHp());
    }
}
