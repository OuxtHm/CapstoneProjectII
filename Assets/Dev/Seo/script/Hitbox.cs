using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{   
     public float damage = 10;
     Enemy enemy;
     Boss boss;
    void OnTriggerEnter2D(Collider2D collider)
    {
        
        if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            enemy = collider.GetComponent<Enemy>();
            StartCoroutine(enemy.Hurt(this.transform, damage));
        }
    }
    
}
