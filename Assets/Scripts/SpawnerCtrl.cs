using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Wave {
    public int simultaneus { get; set; }
    public float delay { get; set; }
    public int max { get; set; }
    public int killed { get; set; } = 0;
    public int spawned { get; set; } = 0;
    public float lastSpawn { get; set; } = 0;
}
public class SpawnerCtrl : MonoBehaviour {
    public int wave = 0;
    public GameObject enemy;

    private GameObject[] spawnPositions;
    private float nextSpawn = 0f;
    private List<Wave> waves = new List<Wave> {
        new Wave() { simultaneus = 2, delay = 3f, max = 10 },
        new Wave() { simultaneus = 5, delay = 3f, max = 20 },
        new Wave() { simultaneus = 10, delay = 3f, max = 30 },
        new Wave() { simultaneus = 15, delay = 3f, max = 40 },
        new Wave() { simultaneus = 20, delay = 3f, max = 50 },
        new Wave() { simultaneus = 25, delay = 3f, max = 70 },
        new Wave() { simultaneus = 25, delay = 2f, max = 100 },
    };

    private void Start() {
        spawnPositions = GameObject.FindGameObjectsWithTag("EnemySpawn");
    }
    private void Update () {
        Spawn();
    }
    private void Spawn() {
        Debug.Log("Max: " + waves[wave].max);
        Debug.Log("Killed: " + waves[wave].killed);
        Debug.Log("Spawned: " + waves[wave].spawned);
        Debug.Log("Simultaneus: " + waves[wave].simultaneus);

        if (waves[wave].max == waves[wave].killed) {
            wave++;

            Debug.Log("Wave: " + wave);
            return;
        }

        if (nextSpawn != 0f && Time.time < nextSpawn) {
            return;
        }

        if (waves[wave].spawned - waves[wave].killed >= waves[wave].simultaneus) {
            return;
        }

        if (waves[wave].spawned == waves[wave].max) {
            return;
        }

        if (nextSpawn == 0f || Time.time > nextSpawn) {
            Debug.Log("Spawning");

            int index = Random.Range(0, spawnPositions.Length);
            GameObject npc = Instantiate(enemy, spawnPositions[index].transform.position, Quaternion.identity);
            EnemyCtrl script = npc.GetComponent<EnemyCtrl>();
            UnityEvent deathEvent = script.GetEvent();
            deathEvent.AddListener(Killed);

            nextSpawn = Time.time + waves[wave].delay;
            waves[wave].spawned++;
        }
    }

    private void Killed() {
        waves[wave].killed++;
    }
}
