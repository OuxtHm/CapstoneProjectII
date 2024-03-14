using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Enemy
{
    public override void InitSetting()
    {
        //common = true;
        enemy_MaxHP = 10;
        enemy_CurHP = 10;
        enemy_Power = 1;
        enemy_Speed = 2;
    }
}
