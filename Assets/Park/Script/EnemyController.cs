using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Enemy enemy;
    public Transform target;

    void Start()
    {
        target = Player.instance.gameObject.transform;
        enemy.InitSetting();
    }

    void Update()
    {
        enemy.Short_Monster(target);
    }
}
