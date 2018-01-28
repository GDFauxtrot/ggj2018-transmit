using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterScript : MonoBehaviour {
    private GameObject player;
    public BulletPoolScriptable bulletPool;
    private bool isShooting;

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	void Update () {
        if (!isShooting)
        {
            isShooting = true;
            Invoke("shootPlayer", 1f);
        }
    }

    private void shootPlayer(){
        Debug.Log("shoot player");
        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
        Vector2 enemyPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 bulletVector = playerPos - enemyPos;
        float bulletAngle = Mathf.Atan2(bulletVector.y, bulletVector.x) * Mathf.Rad2Deg;
        
        Quaternion rotation = Quaternion.Euler(0,0,bulletAngle);
        bulletPool.request(transform.position, rotation, true);
        isShooting = false;
    }
}
