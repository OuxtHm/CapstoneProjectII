using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeGolem : Enemy
{
    public override void InitSetting()
    {
        enemy_Type = 1;
        enemy_MaxHP = 30;
        enemy_CurHP = 30;
        enemy_Power = 5;
        enemy_Speed = 1;
    }
}