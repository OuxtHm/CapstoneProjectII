using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSlime : Enemy
{
    public override void InitSetting()
    {
        enemy_Type = 3;
        enemy_MaxHP = 30;
        enemy_CurHP = 30;
        enemy_Power = 10;
        enemy_Speed = 2;
        enemy_frontSensor = 0.7f;
        enemyMoney = Random.Range(80, 101);
    }
}
