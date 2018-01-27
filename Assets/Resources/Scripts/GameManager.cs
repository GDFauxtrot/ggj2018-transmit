using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyType { SHOOTER, BRAWLER, BOSS };

public class GameManager : MonoBehaviour {

    public GameObject player, inGameCamera, spawnPoints;
    public GameObject shooterPrefab, brawlerPrefab;

    public bool bossSpawned; // To prevent him from spawning again

    public int score;
    public int bossSpawnScore;

    public float spawnPointRange; // How far away can enemies spawn from the spawn points (spawning them exactly on would be boring)

    public float pixelEffectIntensity, pixelEffectTime, pixelEffectFadeTime, lagEffectTime;

    List<GameObject> enemySpawnPoints; // This is all we need, as far as I'm concerned

    InGameCameraManager inGameCameraManager;

    void Start () {
        inGameCameraManager = inGameCamera.GetComponent<InGameCameraManager>();

        foreach (Transform childTransform in spawnPoints.transform) {
            enemySpawnPoints.Add(childTransform.gameObject);
        }
    }
    
    void Update () {
        if (score > bossSpawnScore && !bossSpawned) {
            SpawnBoss();
        }
    }

    public void SpawnBoss() {
        bossSpawned = true;
        // and then do the rest
    }

    public void SendCommand(string message) {
        if (message == "" || message == null)
            return;

        string[] messageSplit = message.Split();

        switch (messageSplit[0]) {
            case "spawn":
                break;
            case "heal":
                break;
            case "pixellate":
                StartCoroutine(PixelEffect(pixelEffectIntensity, pixelEffectTime, pixelEffectFadeTime));
                break;
            case "lag":
                StartCoroutine(LagEffect(lagEffectTime));
                break;
        }
    }

    //                                                                           //
    // -- GameManager Chat Functions and Coroutines (accessed by SendCommand) -- //
    //                                                                           //

    // Preferrably unless the different enemy types need different spawn behaviors,
    // just keep this as generic prefab spawn and pass in the enemy prefab you want
    public void SpawnEnemy(GameObject prefab, int count, bool dontSpawnTogether = false) {
        int spawnPointIndex = Random.Range(0, enemySpawnPoints.Count);

        for (int i = 0; i < count; ++i) {
            if (dontSpawnTogether) // flip this bool to put each one at a different spawn point
                spawnPointIndex = Random.Range(0, enemySpawnPoints.Count);

            Vector2 spawnPos = enemySpawnPoints[spawnPointIndex].transform.position;
            prefab.transform.position = new Vector2(
                spawnPos.x + Random.Range(-spawnPointRange, spawnPointRange),
                spawnPos.y + Random.Range(-spawnPointRange, spawnPointRange));
            Instantiate(prefab);
        }
    }

    private void SetPlayerHealth(int health) {

    }

    private void SetEnemyHealth(EnemyType type, int health) {

    }

    private void SetPlayerSpeed(int speed) {

    }

    private void SetEnemySpeed(EnemyType type, float speed) {

    }

    private void SetPlayerInvincible(float time) {

    }

    private void UpgradePlayerBullets() {

    }

    private void PlayerDamageMultiplier(float multiplier, float time) {

    }

    private void EnemyDamageMultiplier(EnemyType type, float multiplier, float time) {

    }

    private IEnumerator LagEffect(float time) {
        yield return new WaitForSeconds(0f); // need this here or else it complains
    }

    private IEnumerator PixelEffect(float intensity, float time, float fadeTime) {
        float timeStep = 0f;

        while (timeStep < 1f) {
            inGameCameraManager.SetPixelEffectIntensity(Mathf.Lerp(0f, intensity, timeStep));
            yield return new WaitForSeconds(Time.deltaTime);
            timeStep += Time.deltaTime * (1f/fadeTime);
        }
        timeStep = 1f;
        inGameCameraManager.SetPixelEffectIntensity(intensity);
        yield return new WaitForSeconds(time);
        while (timeStep > 0f) {
            inGameCameraManager.SetPixelEffectIntensity(Mathf.Lerp(0f, intensity, timeStep));
            yield return new WaitForSeconds(Time.deltaTime);
            timeStep -= Time.deltaTime * (1f/fadeTime);
        }
        inGameCameraManager.SetPixelEffectIntensity(0f);
    }
}
