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
        // ���̵� �� ����
        fadeScript.Fade(); // 'pade' Ŭ������ 'Fade' �޼��� ȣ��
        // ���̵� ���� ������ ���� ������ ���
        yield return new WaitForSeconds(1f); // ���̵� �ΰ� ��� �ð��� ������ �� �ð�
        // �÷��̾� ��ġ �̵�
        targetObj.transform.position = toObj.transform.position;
        // �߰����� ��� �ð� ���� �ٷ� ���̵� �ƿ� ����
    }
}