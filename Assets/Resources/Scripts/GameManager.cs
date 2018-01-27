using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject player, inGameCamera;

    public GameObject spawnPoints;

    public bool bossSpawned; // To prevent him from spawning again

    public int score;
    public int bossSpawnScore;

    public float spawnPointRange; // How far away can enemies spawn from the spawn points (spawning them exactly on would be boring)

    public float pixelEffectIntensity, pixelEffectTime, pixelEffectFadeTime;

    List<GameObject> enemySpawnPoints; // This is all we need, as far as I'm concerned

    InGameCameraManager inGameCamManager;

    void Start () {
        inGameCamManager = inGameCamera.GetComponent<InGameCameraManager>();

        foreach (Transform childTransform in spawnPoints.transform) {
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

    public void SendCommand(string message) {
        if (message == "" || message == null)
            return;

        string[] messageSplit = message.Split();

        switch (messageSplit[0]) {
            case "spawn":
                break; // check for messageSplit[1] -- spawn what?
            case "summon":
                break; // same as spawn
            case "heal":
                break; // heal the player
            case "distort":
                StartCoroutine(PixelEffect(pixelEffectIntensity, pixelEffectTime, pixelEffectFadeTime));
                break;
        }
    }

    private IEnumerator PixelEffect(float intensity, float time, float fadeTime) {
        float timeStep = 0f;

        while (timeStep < 1f) {
            inGameCamManager.SetPixelEffectIntensity(Mathf.Lerp(0f, intensity, timeStep));
            yield return new WaitForSeconds(Time.deltaTime);
            timeStep += Time.deltaTime * (1f/fadeTime);
        }
        timeStep = 1f;
        inGameCamManager.SetPixelEffectIntensity(intensity);
        yield return new WaitForSeconds(time);
        while (timeStep > 0f) {
            inGameCamManager.SetPixelEffectIntensity(Mathf.Lerp(0f, intensity, timeStep));
            yield return new WaitForSeconds(Time.deltaTime);
            timeStep -= Time.deltaTime * (1f/fadeTime);
        }
        inGameCamManager.SetPixelEffectIntensity(0);
    }
}
