using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyType { SHOOTER, BRAWLER, BOSS };

public class GameManager : MonoBehaviour {

    public GameObject player, inGameCamera, spawnPoints;
    public GameObject shooterPrefab, brawlerPrefab, bossPrefab;

    public bool bossSpawned; // To prevent him from spawning again

    public int score;
    public int bossSpawnScore;

    public float spawnPointRange; // How far away can enemies spawn from the spawn points (spawning them exactly on would be boring)

    public float pixelEffectIntensity, pixelEffectTime, pixelEffectFadeTime, lagEffectTime;

    List<GameObject> enemySpawnPoints; // This is all we need, as far as I'm concerned

    InGameCameraManager inGameCameraManager;

    //The list of booleans for each command
    private bool spawn_brawler_bool = true, heal_bool = true, player_fast_bool = true, enemies_up_damage_bool = true, stream_qual_bool = true,
        spawn_shooter_bool = true, player_double_damage_bool = true, player_slow_bool = true, enemies_slow_bool = true, lag_bool = true,
        bullet_up_bool = true, traps_bool = true, invincible_bool = true, boss_bool = true;
    [Header("Cooldowns")]
    public float spawnBrawlerTimer;
    public float healTimer;
    public float playerSpeedUpTimer;
    public float enemiesDamageUpTimer;
    public float streamQualityTimer;
    public float spawnShooterTimer;
    public float playerDoubleDamageTimer;
    public float playerSpeedDownTimer;
    public float enemySpeedDownTimer;
    public float lagTimer;
    public float bulletUpgradeTimer;
    public float trapsTimer;
    public float invincibleTimer;
    public float bossTimer;

    void Start () {
        inGameCameraManager = inGameCamera.GetComponent<InGameCameraManager>();
        enemySpawnPoints = new List<GameObject>();
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
            case "spawnraptor":
                if(spawn_brawler_bool)
                    StartCoroutine(SpawnEnemy(brawlerPrefab, 3, true));
                break;
            case "heal":
                if (heal_bool)
                    StartCoroutine(SpawnHealthPack());
                break;
            case "playerfast":
                if (player_fast_bool)
                    StartCoroutine(SetPlayerSpeed((int) player.GetComponent<Player>().speed + 2));
                break;
            case "enemydamup":
                if (enemies_up_damage_bool)
                    //StartCoroutine(EnemyDamageMultiplier())*************************************************************
                    print("");
                break;
            case "pixellate":
                if(stream_qual_bool)
                    StartCoroutine(PixelEffect(pixelEffectIntensity, pixelEffectTime, pixelEffectFadeTime));
                break;
            case "spawnshooter":
                if (spawn_shooter_bool)
                    StartCoroutine(SpawnEnemy(shooterPrefab, 3, true));
                break;
            case "playerdamup":
                if (player_double_damage_bool)
                    StartCoroutine(PlayerDamageMultiplier(2, 3));
                break;
            case "playerslow":
                if (player_slow_bool)
                    StartCoroutine(SetPlayerSpeed((int)player.GetComponent<Player>().speed - 2));
                break;
            case "enemyspddown":
                if (enemies_slow_bool)
                    //StartCoroutine(SetEnemySpeed());
                    print("");
                break;
            case "lag":
                if(lag_bool)
                {
                    StartCoroutine(LagEffect(lagEffectTime));
                    StartCoroutine(PixelEffect(2, lagEffectTime, 0.5f));
                }
                break;
            case "bulletup":
                if (bullet_up_bool)
                    StartCoroutine(UpgradePlayerBullets());
                break;
            case "trap":
                if (traps_bool)
                    StartCoroutine(SpawnTrap());
                break;
            case "invince":
                if (invincible_bool)
                    StartCoroutine(PlayerInvincibility(3f));
                break;
            case "boss":
                if (boss_bool)
                    StartCoroutine(SpawnEnemy(bossPrefab, 1, false));
                break;
        }
    }

    //                                                                           //
    // -- GameManager Chat Functions and Coroutines (accessed by SendCommand) -- //
    //                                                                           //

    // Preferrably unless the different enemy types need different spawn behaviors,
    // just keep this as generic prefab spawn and pass in the enemy prefab you want
    private IEnumerator SpawnEnemy(GameObject prefab, int count, bool dontSpawnTogether = false) {
        spawn_brawler_bool = false;
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
        yield return new WaitForSeconds(spawnBrawlerTimer);
        spawn_brawler_bool = true;
    }
    
    //Idk if we are using either of these because there might be a pickup for the player
    //private IEnumerator SetPlayerHealth(int health) {
    //    heal_bool = false;
    //    player.GetComponent<Player>().SetHealth(health);
    //    yield return new WaitForSeconds(healTimer);
    //    heal_bool = true;
    //}

    //private IEnumerator AddPlayerHealth(int addHealth) {
    //    player.GetComponent<Player>().SetHealth(player.GetComponent<Player>().health + addHealth);
    //}

    //private IEnumerator SetEnemyHealth(EnemyType type, int health) {
        
    //}

    private IEnumerator SpawnHealthPack()
    {
        heal_bool = false;
        yield return new WaitForSeconds(healTimer);
        heal_bool = true;
    }

    private IEnumerator SetPlayerSpeed(int speed) {
        if(speed > player.GetComponent<Player>().speed)
        {
            player_fast_bool = false;
            player.GetComponent<Player>().speed = speed;
            yield return new WaitForSeconds(playerSpeedUpTimer);
            player_fast_bool = true;
        }
        else
        {
            player_slow_bool = false;
            player.GetComponent<Player>().speed = speed;
            yield return new WaitForSeconds(playerSpeedDownTimer);
            player_slow_bool = true;
        }
    }

    private IEnumerator SetEnemySpeed(EnemyType type, float speed) {
        player_fast_bool = false;
        yield return new WaitForSeconds(playerSpeedUpTimer);
        player_fast_bool = true;
    }

    private IEnumerator SetPlayerInvincible(float time) {
        invincible_bool = false;
        StartCoroutine(PlayerInvincibility(time));
        yield return new WaitForSeconds(invincibleTimer);
        invincible_bool = true;
    }

    private IEnumerator UpgradePlayerBullets() {
        bullet_up_bool = false;
        yield return new WaitForSeconds(bulletUpgradeTimer);
        bullet_up_bool = true;
    }

    // double damage, half damage, etc
    private IEnumerator PlayerDamageMultiplier(float multiplier, float time) {
        player_double_damage_bool = false;
        yield return new WaitForSeconds(playerDoubleDamageTimer);
        player_double_damage_bool = true;
    }

    private IEnumerator EnemyDamageMultiplier(EnemyType type, float multiplier, float time) {
        enemies_up_damage_bool = false;
        yield return new WaitForSeconds(enemiesDamageUpTimer);
        enemies_up_damage_bool = true;
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

    private IEnumerator SpawnTrap()
    {
        traps_bool = false;
        yield return new WaitForSeconds(trapsTimer);
        traps_bool = true;
    }
}
