using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject targetObj;
    public pade fadeScript;
    public GameObject toObj;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            targetObj = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(TelepotyRoutine());
        }
    }

    IEnumerator TelepotyRoutine()
    {
        // 페이드 인 실행
        fadeScript.Fade(); // 'pade' 클래스의 'Fade' 메서드 호출
        // 페이드 인이 완전히 끝날 때까지 대기
        yield return new WaitForSeconds(1f); // 페이드 인과 대기 시간을 포함한 총 시간
        // 플레이어 위치 이동
        targetObj.transform.position = toObj.transform.position;
        // 추가적인 대기 시간 없이 바로 페이드 아웃 시작
    }
}