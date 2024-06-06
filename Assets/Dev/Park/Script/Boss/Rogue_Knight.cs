using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rogue_Knight : Boss
{
    private bool isDying = false; // ������ �״� ������ Ȯ���ϴ� �÷���
    public override void BossInitSetting()
    {
        boss_stage = 2;
        boss_MaxHP = 100;
        boss_CurHP = 100;
        boss_Speed = 5;
        boss_BumpPower = 10;    //�浹 �����
        boss_OnePattenPower = 20;   // ���� �����̵� �������� ���� �����
        boss_TwoPattenPower = 30;   //  ���κ��� ���� �����
        boss_ThreePattenPower = 20;    //   ����Ʈ �Ҳ� ���� �����
        boss_FourPattenPower = 40;   //
    }

    void Update()
    {
        // boss_CurHP�� 0 �����̰�, ���� �״� ���� �ƴ϶�� Die �ڷ�ƾ�� �����մϴ�.
        if (boss_CurHP <= 0 && !isDying)
        {
            StartCoroutine(pt2());
        }
    }//5/31 �̰�� �߰�

    IEnumerator pt2()
    {
        isDying = true; // �״� ������ ���� ����

        StageManager.instance.ActivatePortal2();

        yield return new WaitForSeconds(3.0f); // �ʿ信 ���� ��� �ð� ����

        isDying = false; // ���� ó�� �Ϸ�
    }//5/31 �̰�� �߰�
}
