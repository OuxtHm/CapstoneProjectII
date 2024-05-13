using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSkill : MonoBehaviour
{
    public static ChangeSkill instance;
    public Image changeCoolTime;

    public Image skill_1;  // 스킬 오브젝트
    public Image skill_2;  // 스킬 오브젝트

    public Transform nowskill;     // 사용 중인 스킬
    public Transform readyskill;   // 대기 중인 스킬

    public bool change;     // 스킬 변경 했는지 확인하는 변수
    private void Awake()
    {
        instance = this;
        changeCoolTime = transform.GetChild(0).GetComponent<Image>();
        nowskill = transform.GetChild(1).GetComponent<Transform>();     // 현재 사용중인 스킬 Transform 설정
        readyskill = transform.GetChild(2).GetComponent<Transform>();   // 대기중인 스킬 Transform 설정
        skill_1 = nowskill.GetChild(0).GetComponent<Image>();       // 사용중인 스킬 Image 설정
        skill_2 = readyskill.GetChild(0).GetComponent<Image>();     // 대기중인 스킬 Image 설정
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

    public IEnumerator ChangeskillCoolTime(float duration)      // 기본 스킬 변경 쿨타임 적용
    {
        change = true;
        float elapsedTime = 0f; // 경과 시간
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

    public void Swapskill()     // 기본 스킬 이미지 위치 교환
    {
        nowskill = transform.GetChild(1).GetComponent<Transform>();     // 현재 사용중인 스킬 Transform 설정
        readyskill = transform.GetChild(2).GetComponent<Transform>();   // 대기중인 스킬 Transform 설정

        skill_1 = nowskill.GetChild(0).GetComponent<Image>();       // 사용중인 스킬 Image 설정
        skill_2 = readyskill.GetChild(0).GetComponent<Image>();     // 대기중인 스킬 Image 설정

        skill_1.transform.SetParent(readyskill.transform, false);   // 대기중인 스킬 사용중 슬롯으로 이동
        skill_1.transform.SetAsFirstSibling();                          // 부모 오브젝트의 첫번째 자식으로 설정

        skill_2.transform.SetParent(nowskill.transform, false);     // 사용중인 스킬 대기중 슬롯으로 이동
        skill_2.transform.SetAsFirstSibling();                          // 부모 오브젝트의 첫번째 자식으로 설정
    }
}
