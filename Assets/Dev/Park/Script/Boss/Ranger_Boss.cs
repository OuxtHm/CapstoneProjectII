using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger_Boss : Boss
{
    public override void BossInitSetting()
    {
        boss_MaxHP = 100;
        boss_CurHP = 100;
        boss_Power = 20;
        boss_Speed = 1;
    }
}
