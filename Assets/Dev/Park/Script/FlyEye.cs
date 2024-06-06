using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEye : Enemy
{
    public override void InitSetting()
    {
        enemy_Type = 2;
        enemy_MaxHP = 10;
        enemy_CurHP = 10;
        enemy_Power = 1;
        enemy_Speed = 2;
        enemyMoney = Random.Range(80, 101);
        detectionRange = 15f;
    }
}
