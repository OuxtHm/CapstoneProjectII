using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger_Boss : Boss
{
    private bool isDying = false; // ������ �״� ������ Ȯ���ϴ� �÷���
    public override void BossInitSetting()
    {
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
    void Update()
    {

        // boss_CurHP�� 0 �����̰�, ���� �״� ���� �ƴ϶�� Die �ڷ�ƾ�� �����մϴ�.
        if (boss_CurHP <= 0 && !isDying)
        {
            StartCoroutine(pt1());
        }
        else
        {
            StageManager.instance.ExitPortal1();
        }
    }//5/31 �̰�� �߰�

    IEnumerator pt1()
    {
        isDying = true; // �״� ������ ���� ����

        StageManager.instance.ActivatePortal1();

        yield return new WaitForSeconds(3.0f); // �ʿ信 ���� ��� �ð� ����

        isDying = false; // ���� ó�� �Ϸ�
    }//5/31 �̰�� �߰�
}