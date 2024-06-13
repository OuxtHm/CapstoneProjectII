using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramses : Enemy
{
    public override void InitSetting()
    {
        enemy_Type = 3;
        enemy_MaxHP = 10;
        enemy_CurHP = 10;
        enemy_Power = 2;
        enemy_Speed = 3;
        enemy_frontSensor = 1.3f;
        enemyMoney = Random.Range(50, 71);
    }
}
