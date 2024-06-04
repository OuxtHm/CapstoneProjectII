using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSlime : Boss
{
    public override void BossInitSetting()
    {
        boss_stage = 4;
        boss_MaxHP = 100;
        boss_CurHP = 100;
        boss_Speed = 2;
        boss_BumpPower = 15;    //충돌 대미지
    }
}
