using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Enemy
{
    public override void InitSetting()
    {
        //common = true;
        enemy_MaxHP = 20;
        enemy_CurHP = 20;
        enemy_Power = 3;
        enemy_Speed = 3;
    }
}
