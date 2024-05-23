using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossController : MonoBehaviour
{


    public Boss boss;
    public Transform target;


    void Start()
    {
        target = Player.instance.gameObject.transform;
        boss.BossInitSetting();
    }

    void Update()
    {
        boss.BossUpdate(target);
    }
}