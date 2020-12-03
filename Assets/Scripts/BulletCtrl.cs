using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour {
    public GameObject hitEffect;
    private int damage;
    
    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("Colliding with: " + collision.gameObject.name);

        if (hitEffect != null) {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 5f);
        }

        if (collision.gameObject.tag == "Enemy") {
            EnemyCtrl ctrl = collision.gameObject.GetComponent<EnemyCtrl>();
            
            Debug.Log("Hitting enemy with " + damage + " damage");
            

            ctrl.TakeDamage(damage);
        }
        Destroy(gameObject);
    }

    public void SetDamage(int dmg) {
        damage = dmg;
    }
}
