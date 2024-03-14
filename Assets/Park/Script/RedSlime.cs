using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSlime : Enemy
{
    public override void InitSetting()
    {
        //common = true;
        enemy_MaxHP = 30;
        enemy_CurHP = 30;
        enemy_Power = 10;
        enemy_Speed = 2;
    }
}
