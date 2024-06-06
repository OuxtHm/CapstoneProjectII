using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot_Plant : Enemy
{
    public override void InitSetting()
    {
        enemy_Type = 4;
        enemy_MaxHP = 10;
        enemy_CurHP = 10;
        enemy_Power = 2;
        enemy_Speed = 0;
        enemy_AttackSensor = 7f;
        enemyMoney = Random.Range(80, 101);
    }
}
