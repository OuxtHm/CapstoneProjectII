using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenSlime : Enemy
{
    public override void InitSetting()
    {
        enemy_Type = 1;
        enemy_MaxHP = 10;
        enemy_CurHP = 10;
        enemy_Power = 1;
        enemy_Speed = 1;
    }
}