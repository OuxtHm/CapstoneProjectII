using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject targetObj;
    public pade fadeScript; // 'pade'가 올바른 클래스 이름인지 확인하세요. 일반적으로 클래스 이름은 대문자로 시작합니다.
    public GameObject toObj;

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (animator != null) // Animator 컴포넌트가 존재하는지 확인
            {
                animator.SetBool("gate", true);
            }
            targetObj = collision.gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.X))
        {
            StartCoroutine(TelepotyRoutine());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (animator != null) // Animator 컴포넌트가 존재하는지 확인
        {
            animator.SetBool("gate", false);
        }
    }

    IEnumerator TelepotyRoutine()
    {
        // 페이드 인 실행
        fadeScript.Fade(); // 'pade' 클래스의 'Fade' 메서드 호출. 클래스 이름이 'Fade'로 올바르게 되어 있는지 확인하세요.
        // 페이드 인이 완전히 끝날 때까지 대기
        yield return new WaitForSeconds(1f); // 페이드 인과 대기 시간을 포함한 총 시간
        // 플레이어 위치 이동
        targetObj.transform.position = toObj.transform.position;
        // 추가적인 대기 시간 없이 바로 페이드 아웃 시작
    }
}
