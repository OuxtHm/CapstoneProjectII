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
        boss_BumpPower = 5;    //충돌 대미지
        boss_OnePattenPower = 10;   // 투명 순간이동 근접공격 패턴 대미지
        boss_TwoPattenPower = 20;   //  가로베기 패턴 대미지
        boss_ThreePattenPower = 30;    //   라이트 불꽃 패턴 대미지
        boss_FourPattenPower = 40;   //
    }
}
