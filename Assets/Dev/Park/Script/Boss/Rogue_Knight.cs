using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rogue_Knight : Boss
{
    public override void BossInitSetting()
    {
        boss_stage = 2;
        boss_MaxHP = 100;
        boss_CurHP = 100;
        boss_Speed = 5;
        boss_BumpPower = 5;    //�浹 �����
        boss_OnePattenPower = 10;   // ���� �����̵� �������� ���� �����
        boss_TwoPattenPower = 20;   //  ���κ��� ���� �����
        boss_ThreePattenPower = 30;    //   ����Ʈ �Ҳ� ���� �����
        boss_FourPattenPower = 40;   //
    }
}
