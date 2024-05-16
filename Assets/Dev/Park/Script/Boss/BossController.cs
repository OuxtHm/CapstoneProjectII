using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossController : MonoBehaviour
{

    public Boss boss;
    public Transform target;
    public static Ranger_Boss instance;
    Ranger_Boss ranger_Boss;

    void Start()
    {
        target = Player.instance.gameObject.transform;
        boss.BossInitSetting();
        ranger_Boss = boss as Ranger_Boss; // boss 객체가 Ranger_Boss 타입이라면 형변환을 통해 ranger_Boss를 초기화합니다.
    }

    void Update()
    {
        boss.BossUpdate(target);

        // Check if the boss's health is 0 or less and ranger_Boss is not null
        if (ranger_Boss != null && ranger_Boss.boss_CurHP <= 0)
        {
            // Call the OnDeath method of the boss
            ranger_Boss.OnDeath();

            // Optionally, destroy the boss game object
            Destroy(gameObject);
        }
    }

}