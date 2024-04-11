using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject targetObj;
    public pade fadeScript; // 'pade'�� �ùٸ� Ŭ���� �̸����� Ȯ���ϼ���. �Ϲ������� Ŭ���� �̸��� �빮�ڷ� �����մϴ�.
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
            if (animator != null) // Animator ������Ʈ�� �����ϴ��� Ȯ��
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
        if (animator != null) // Animator ������Ʈ�� �����ϴ��� Ȯ��
        {
            animator.SetBool("gate", false);
        }
    }

    IEnumerator TelepotyRoutine()
    {
        // ���̵� �� ����
        fadeScript.Fade(); // 'pade' Ŭ������ 'Fade' �޼��� ȣ��. Ŭ���� �̸��� 'Fade'�� �ùٸ��� �Ǿ� �ִ��� Ȯ���ϼ���.
        // ���̵� ���� ������ ���� ������ ���
        yield return new WaitForSeconds(1f); // ���̵� �ΰ� ��� �ð��� ������ �� �ð�
        // �÷��̾� ��ġ �̵�
        targetObj.transform.position = toObj.transform.position;
        // �߰����� ��� �ð� ���� �ٷ� ���̵� �ƿ� ����
    }
}
