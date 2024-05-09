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
        boss_Power = 10;
        boss_BumpPower = 15;    //�浹 �����
        boss_OnePattenPower = 10;   //�������� ���� �����
        boss_TwoPattenPower = 20;   //ȭ�� ���� �����
        boss_ThreePattenPower = 30;    //ȭ��� ���� �����
        boss_FourPattenPower = 40;   //������ ���� �����

    }
}
