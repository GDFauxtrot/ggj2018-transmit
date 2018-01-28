using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DumbEnemyAI : MonoBehaviour {
    private GameObject player;
    private Player playerScript;
    private Rigidbody2D enemyRB;
    public GameObject death_explosion;

    public int enemyHealth = 3;
    [Tooltip("Determines how fast the enemy will move towards the player. Values: [0-1], 0 means no movement at all, and 1 means teleport to player.")]
    public float moveDelta = .05f;
    [Tooltip("How much damage the player will take when the enemy hits them.")]
    public int damage = 5;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyRB = transform.GetComponent<Rigidbody2D>();
    }

	// Update is called once per frame
	void Update () {
        move();                     // Moves the enemy towards the player.
        checkIfDead();              // Checks if health has fallen below 0, and destroys the GameObject.
	}

    // Move this into a Brawler script.
    public void OnTriggerEnter2D(Collider2D other)
    {
        // Please make sure the player is tagged as "Player"!!!
        if (other.tag == "Player")
        {
            if (playerScript == null)
            {
                playerScript = player.GetComponent<Player>();
            }
            playerScript.SetHealth(playerScript.health - damage);
        }
    }

    //Private Functions: Nothing else needs to call these, so im making them private.
    private void move() {
        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
        Vector2 enemyPos = new Vector2(transform.position.x, transform.position.y);

        enemyRB.MovePosition(Vector2.MoveTowards(enemyPos, playerPos, moveDelta));
    }

    private void checkIfDead() {
        if (enemyHealth <= 0) {
            Instantiate(death_explosion, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
