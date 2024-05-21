using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger_Boss : Boss
{
    public GameObject coinPrefab; // ���� ������ ������ ���� ����
    public GameObject potionPrefab; // ���� ������ ������ ���� ����
    public GameObject skillItemPrefab; // ��ų ������ ������ ������ ���� ����

    public int numberOfCoins = 10; // ������ ������ ��
    public int numberOfPotions = 2; // ������ ������ ��
    public int numberOfSkillItems = 1; // ������ ��ų �������� ��
    public float scatterRadius = 1.0f; // �������� ������� �ݰ�

    public override void BossInitSetting()
    {
        // ���� ���� �ڵ�...
        boss_stage = 1;
        boss_MaxHP = 100;
        boss_CurHP = 100;
        boss_Speed = 3;
        boss_BumpPower = 5;
        boss_OnePattenPower = 10;   //���� ���� ���� �����
        boss_TwoPattenPower = 20;   // �⺻ Ȱ ���� �����
        boss_ThreePattenPower = 20; // ȭ��� ���� �����
        boss_FourPattenPower = 30;  // ������ ���� �����
    }

    // ������ �׾��� �� ȣ��� �޼���
    public void OnDeath()
    {
        DropItems(coinPrefab, numberOfCoins);
        DropItems(potionPrefab, numberOfPotions);
        DropItems(skillItemPrefab, numberOfSkillItems);

        Debug.Log("������ ������");
    }

    void DropItems(GameObject itemPrefab, int numberOfItems)
    {
        for (int i = 0; i < numberOfItems; i++)
        {
            Vector2 scatterOffset = Random.insideUnitCircle.normalized * scatterRadius * Random.Range(0.5f, 1.5f);
            Vector3 dropPosition = transform.position + new Vector3(scatterOffset.x, scatterOffset.y, 0);

            // �������� �ν��Ͻ�ȭ
            GameObject droppedItem = Instantiate(itemPrefab, dropPosition, Quaternion.identity);

            // Rigidbody2D ������Ʈ�� �������� �߰�
            Rigidbody2D rb = droppedItem.AddComponent<Rigidbody2D>();
            // �ʿ��ϴٸ� �ٸ� Rigidbody2D ������ ������ �� �ֽ��ϴ�
            // ��: rb.gravityScale = 1f;
        }
    }
}
