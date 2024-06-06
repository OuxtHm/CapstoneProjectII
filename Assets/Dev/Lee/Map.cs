using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField]
    private Transform[] backgrounds; // �� �� �̻��� ����� �迭�� ����
    [SerializeField]
    private float scrollAmount; // ����� �ʺ�
    [SerializeField]
    private Vector3 moveDirection; // �̵� ����

    private Transform target;

    private void Awake()
    {
        target = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        foreach (Transform background in backgrounds)
        {
            // �÷��̾ ����� �������� ������ ��
            if (target.position.x >= background.position.x + scrollAmount)
            {
                background.position += moveDirection * (backgrounds.Length * scrollAmount);
            }

            // �÷��̾ ����� �������� ������ ��
            if (target.position.x <= background.position.x - scrollAmount)
            {
                background.position -= moveDirection * (backgrounds.Length * scrollAmount);
            }
        }
    }
}