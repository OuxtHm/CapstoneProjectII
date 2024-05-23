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
        boss_BumpPower = 15;    //충돌 대미지
        boss_OnePattenPower = 20;   //근접공격 패턴 대미지
        boss_TwoPattenPower = 20;   //배리어 패턴 대미지
        boss_ThreePattenPower = 10;    //파이어볼트 패턴 대미지
        boss_FourPattenPower = 30;   //파이어브레스 패턴 대미지

    }
}
