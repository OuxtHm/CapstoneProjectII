using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canine : Enemy
{
    public override void InitSetting()
    {
        enemy_Type = 1;
        enemy_MaxHP = 10;
        enemy_CurHP = 10;
        enemy_Power = 2;
        enemy_Speed = 3;
        enemy_AttackSensor = 1.3f;
        enemy_frontSensor = 1f;
    }
}
