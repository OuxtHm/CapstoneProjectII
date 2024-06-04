using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultistPriest : Enemy
{
    public override void InitSetting()
    {
        enemy_Type = 1;
        enemy_MaxHP = 10;
        enemy_CurHP = 10;
        enemy_Power = 2;
        enemy_Speed = 1;
        enemy_AttackSensor = 3f;
        enemy_frontSensor = 1.5f;
        enemyMoney = Random.Range(80, 101);
    }
}
