using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Enemy
{
    public override void InitSetting()
    {
        enemy_Type = 1;
        enemy_MaxHP = 10;
        enemy_CurHP = 10;
        enemy_Power = 1;
        enemy_Speed = 2;
        enemy_AttackSensor = 0.8f;
        enemy_frontSensor = 1f;
        enemyMoney = Random.Range(20, 31);
    }
}
