using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon_Boss : Boss
{
    private bool isDying = false; // ������ �״� ������ Ȯ���ϴ� �÷���
    public override void BossInitSetting()
    {
        boss_stage = 3;
        boss_MaxHP = 100;
        boss_CurHP = 100;
        boss_Speed = 2;
        boss_BumpPower = 15;    //�浹 �����
        boss_OnePattenPower = 20;   //�������� ���� �����
        boss_TwoPattenPower = 20;   //�踮�� ���� �����
        boss_ThreePattenPower = 10;    //���̾Ʈ ���� �����
        boss_FourPattenPower = 30;   //���̾�극�� ���� �����

    }

    void Update()
    {
        // boss_CurHP�� 0 �����̰�, ���� �״� ���� �ƴ϶�� Die �ڷ�ƾ�� �����մϴ�.
        if (boss_CurHP <= 0 && !isDying)
        {
            StartCoroutine(pt3());
        }
    }//5/31 �̰�� �߰�

    IEnumerator pt3()
    {
        isDying = true; // �״� ������ ���� ����

        StageManager.instance.ActivatePortal3();

        yield return new WaitForSeconds(3.0f); // �ʿ信 ���� ��� �ð� ����

        isDying = false; // ���� ó�� �Ϸ�
    }//5/31 �̰�� �߰�
}
