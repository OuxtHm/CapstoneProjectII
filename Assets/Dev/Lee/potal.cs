using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    // �÷��̾ �̵���ų ��ġ
    public Vector2 targetPosition = new Vector2(197.84f, 3.23f);

    // Ʈ���ſ� �ٸ� ������Ʈ�� ������ �� �ڵ����� ȣ��Ǵ� �޼���
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ���� ������Ʈ�� �±װ� "Player"���� Ȯ��
        if (other.CompareTag("Player"))
        {
            // �÷��̾��� ��ġ�� targetPosition���� ����
            other.transform.position = targetPosition;
        }
    }
}
