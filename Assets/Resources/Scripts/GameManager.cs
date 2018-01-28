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

    //The list of booleans for each command
    private bool spawn_brawler, heal, player_fast, enemies_up_damage, stream_qual, spawn_shooter, player_double_damage, player_slow, enemies_slow, lag, bullet_up, traps, invincible, boss = false;

    void Start () {
        inGameCameraManager = inGameCamera.GetComponent<InGameCameraManager>();
        enemySpawnPoints = new List<GameObject>();
        foreach (Transform childTransform in spawnPoints.transform) {
            print("hi");
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
            case "spawnraptor":
                SpawnEnemy(brawlerPrefab, 3, true);
                break;
            case "heal":
                break;
            case "pixellate":
                StartCoroutine(PixelEffect(pixelEffectIntensity, pixelEffectTime, pixelEffectFadeTime));
                break;
            case "lag":
                StartCoroutine(LagEffect(lagEffectTime));
                StartCoroutine(PixelEffect(2, lagEffectTime, 0.5f));
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
        player.GetComponent<Player>().SetHealth(health);
    }

    private void AddPlayerHealth(int addHealth) {
        player.GetComponent<Player>().SetHealth(player.GetComponent<Player>().health + addHealth);
    }

    private void SetEnemyHealth(EnemyType type, int health) {

    }

    private void SetPlayerSpeed(int speed) {
        player.GetComponent<PlayerMovement>().speed = speed;
    }

    private void SetEnemySpeed(EnemyType type, float speed) {

    }

    private void SetPlayerInvincible(float time) {
        StartCoroutine(PlayerInvincibility(time));
    }

    private void UpgradePlayerBullets() {

    }

    // double damage, half damage, etc
    private void PlayerDamageMultiplier(float multiplier, float time) {

    }

    private void EnemyDamageMultiplier(EnemyType type, float multiplier, float time) {

    }

    private IEnumerator PlayerInvincibility(float time) {
        player.GetComponent<Player>().isInvincible = true;
        yield return new WaitForSeconds(time);
        player.GetComponent<Player>().isInvincible = false;
    }

    private IEnumerator LagEffect(float time) {
        float currentTime = 0f;

        while (currentTime < time) {
            bool waitThisCycle = Random.Range(0, 60) == 0;

            if (waitThisCycle) {
                float wait = Random.Range(0.1f, 1f);
                Time.timeScale = 0f;
                yield return new WaitForSecondsRealtime(wait);
                Time.timeScale = 1f;
                currentTime += wait;
            } else {
                yield return new WaitForSeconds(Time.deltaTime);
                currentTime += Time.deltaTime;
            }
        }
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
