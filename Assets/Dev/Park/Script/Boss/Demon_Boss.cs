using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon_Boss : Boss
{
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
}
