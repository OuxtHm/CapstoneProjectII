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
        boss_BumpPower = 10;    //�浹 �����
        boss_OnePattenPower = 20;   // ���� �����̵� �������� ���� �����
        boss_TwoPattenPower = 30;   //  ���κ��� ���� �����
        boss_ThreePattenPower = 20;    //   ����Ʈ �Ҳ� ���� �����
        boss_FourPattenPower = 40;   //
    }
}
