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
        boss_BumpPower = 5;    //충돌 대미지
        boss_OnePattenPower = 10;   //근접공격 패턴 대미지
        boss_TwoPattenPower = 20;   //화살 패턴 대미지
        boss_ThreePattenPower = 30;    //화살비 패턴 대미지
        boss_FourPattenPower = 40;   //레이져 패턴 대미지

    }
}
