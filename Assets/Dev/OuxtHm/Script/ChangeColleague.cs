using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColleague : MonoBehaviour
{
    public Image changeCoolTime;

    public Image colleague_1;  // 동료 오브젝트
    public Image colleague_2;  // 동료 오브젝트

    public Transform nowColleague;     // 사용 중인 캐릭터
    public Transform readyColleague;   // 대기 중인 캐릭터

    public bool change;     // 캐릭터 변경 했는지 확인하는 변수
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

    public IEnumerator ChangeColleagueCoolTime()      // 동료 변경 쿨타임 적용
    {
        change = true;
        float duration = 7f; // 보간에 걸리는 시간 (초)
        float elapsedTime = 0f; // 경과 시간
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

    public void SwapColleague()     // 동료 이미지 위치 교환
    {
        nowColleague = transform.GetChild(0).GetComponent<Transform>();     // 현재 사용중인 캐릭터 Transform 설정
        readyColleague = transform.GetChild(1).GetComponent<Transform>();   // 대기중인 캐릭터 Transform 설정

        colleague_1 = nowColleague.GetChild(0).GetComponent<Image>();       // 사용중인 캐릭터 Image 설정
        colleague_2 = readyColleague.GetChild(0).GetComponent<Image>();     // 대기중인 캐릭터 Transform 설정

        colleague_1.transform.SetParent(readyColleague.transform, false);   // 대기중인 캐릭터를 사용중 슬롯으로 이동
        colleague_1.transform.SetAsFirstSibling();                          // 부모 오브젝트의 첫번째 자식으로 설정

        colleague_2.transform.SetParent(nowColleague.transform, false);     // 사용중인 캐릭터를 대기중 슬롯으로 이동
        colleague_2.transform.SetAsFirstSibling();                          // 부모 오브젝트의 첫번째 자식으로 설정
    }   
}
