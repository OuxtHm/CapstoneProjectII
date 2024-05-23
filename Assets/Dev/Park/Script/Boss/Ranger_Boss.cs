using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger_Boss : Boss
{
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
}