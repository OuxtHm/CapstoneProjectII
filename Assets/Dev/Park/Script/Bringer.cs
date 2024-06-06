using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bringer : Enemy
{
    public override void InitSetting()
    {
        enemy_Type = 1;
        enemy_MaxHP = 50;
        enemy_CurHP = 50;
        enemy_Power = 10;
        enemy_Speed = 2;
        enemy_AttackSensor = 1.5f;
        enemy_frontSensor = 1.5f;
    }
}
