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
        boss_OnePattenPower = 10;   //근접 공격 패턴 대미지
        boss_TwoPattenPower = 20;   // 기본 활 패턴 대미지
        boss_ThreePattenPower = 20; // 화살비 패턴 대미지
        boss_FourPattenPower = 30;  // 레이져 패턴 대미지
    }
}