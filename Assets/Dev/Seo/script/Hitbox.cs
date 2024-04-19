using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{   
     public int damage = 10;
     Enemy enemy;
    Boss boss;
    void OnTriggerEnter2D(Collider2D collider)
    {
        
        if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            enemy = collider.GetComponent<Enemy>();
            collider.GetComponent<Enemy>().StartCoroutine(enemy.Hurt(this.transform, damage));
        }
        if (collider.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            boss = collider.GetComponent<Boss>();
            collider.GetComponent<Boss>().StartCoroutine(boss.Hurt(this.transform, damage));
        }
    }
    
}
