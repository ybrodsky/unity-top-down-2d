using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Events;

public class EnemyCtrl : MonoBehaviour {
    public int health = 10;
    public int damage = 10;
    public int speed = 10;

    UnityEvent deathEvent = new UnityEvent();

    private void Awake() {
        AIDestinationSetter script = gameObject.GetComponent<AIDestinationSetter>();

        GameObject player = GameObject.FindWithTag("Player");
        script.target = player.transform;
    }

    void Update() {
        
    }

    public void TakeDamage(int amount) {
        damage -= amount;

        if (damage <= 0) {
            Destroy(gameObject);
            deathEvent.Invoke();
        }
    }

    public UnityEvent GetEvent() {
        return deathEvent;
    }
}
