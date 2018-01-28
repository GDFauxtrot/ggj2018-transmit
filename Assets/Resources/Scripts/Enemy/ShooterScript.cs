using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterScript : MonoBehaviour {
    private GameObject player;
    public BulletPoolScriptable bulletPool;
    private bool can_shoot = true;

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	void Update () {
        shootPlayer();
    }

    private void shootPlayer(){
        if(can_shoot)
        {
            Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
            Vector2 enemyPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 bulletVector = playerPos - enemyPos;
            float bulletAngle = Mathf.Atan2(bulletVector.y, bulletVector.x) * Mathf.Rad2Deg;

            Quaternion rotation = Quaternion.Euler(0, 0, bulletAngle);
            bulletPool.request(transform.position, rotation, "enemy");
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        can_shoot = false;
        yield return new WaitForSeconds(GetComponent<DumbEnemyAI>().reloadTime);
        can_shoot = true;
    }
}
