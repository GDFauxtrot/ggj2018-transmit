using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject player;

    public bool bossSpawned; // To prevent him from spawning again

    public int score;
    public int bossSpawnScore;

    public float spawnPointRange; // How far away can enemies spawn from the spawn points (spawning them exactly on would be boring)

    private List<GameObject> enemySpawnPoints; // This is all we need, as far as I'm concerned

    void Start () {
        foreach (Transform childTransform in GameObject.Find("spawnPointParent").transform) {
            enemySpawnPoints.Add(childTransform.gameObject);
        }
    }
    
    void Update () {
        if (score > bossSpawnScore && !bossSpawned) {
            SpawnBoss();
        }
    }

    public void SpawnRaptors(int count, bool dontSpawnTogether = false) {
        GameObject raptorPrefab = Resources.Load<GameObject>("Prefabs/Raptor");
        int spawnPointIndex = Random.Range(0, enemySpawnPoints.Count);

        for (int i = 0; i < count; ++i) {
            if (dontSpawnTogether) // flip this bool to put each raptor at a different spawn point
                spawnPointIndex = Random.Range(0, enemySpawnPoints.Count);

            Vector2 spawnPos = enemySpawnPoints[spawnPointIndex].transform.position;
            raptorPrefab.transform.position = new Vector2(
                spawnPos.x + Random.Range(-spawnPointRange, spawnPointRange),
                spawnPos.y + Random.Range(-spawnPointRange, spawnPointRange));
            Instantiate(raptorPrefab);
        }
    }

    public void SpawnBoss() {
        bossSpawned = true;
    }
}
