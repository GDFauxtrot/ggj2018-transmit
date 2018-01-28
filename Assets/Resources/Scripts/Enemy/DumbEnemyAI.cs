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
    public float reloadTime = 2;

    private AudioSource audio_player;
    public AudioClip[] sounds;

    private Animator anim;

    public bool can_damage = true, animate = true, boss = false;

    public SpriteRenderer sr;

    void Start() {
        anim = GetComponent<Animator>();
        audio_player = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemyRB = transform.GetComponent<Rigidbody2D>();
        StartCoroutine(Make_Sounds());
    }

	// Update is called once per frame
	void Update () {
        move();                     // Moves the enemy towards the player.
        checkIfDead();              // Checks if health has fallen below 0, and destroys the GameObject.
	}

    //// Move this into a Brawler script.
    //public void OnTriggerEnter2D(Collider2D other)
    //{
    //    // Please make sure the player is tagged as "Player"!!!
    //    if (other.tag == "Player")
    //    {
    //        if (playerScript == null)
    //        {
    //            playerScript = player.GetComponent<Player>();
    //        }
    //        playerScript.TakeDamage(damage);
    //    }
    //}

    //Private Functions: Nothing else needs to call these, so im making them private.
    private void move() {
        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
        Vector2 enemyPos = new Vector2(transform.position.x, transform.position.y);


        if(animate)
        {
            Vector2 diff = (playerPos - enemyPos).normalized;
            anim.SetInteger("Direction", -1);
            if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
            {
                //Moving left and right
                if (diff.x >= 0)
                {
                    anim.SetInteger("Direction", 0);
                    sr.flipX = true;
                }
                else
                {
                    anim.SetInteger("Direction", 2);
                    sr.flipX = false;
                }
            }
            else
            {
                //Moving up and down
                if (diff.y >= 0)
                    anim.SetInteger("Direction", 1);
                else
                    anim.SetInteger("Direction", 3);
            }
        }


        enemyRB.MovePosition(Vector2.MoveTowards(enemyPos, playerPos, moveDelta));
    }

    private void checkIfDead() {
        if (enemyHealth <= 0 && !boss) {
            PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + 50);
            Instantiate(death_explosion, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
            Destroy(this.gameObject);
        }
        else if(boss && enemyHealth <= 0)
        {
            StartCoroutine(transform.GetComponent<Boss_Shooting>().Death());
            GetComponent<DumbEnemyAI>().enabled = false;
        }
    }

    private IEnumerator Make_Sounds()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 5f));
            audio_player.clip = sounds[Random.Range(0, sounds.Length)];
            audio_player.pitch = Random.Range(0.9f, 1.1f);
            audio_player.volume = 0.8f;
            audio_player.Play();
        }
    }

    void OnCollisionStay2D(Collision2D c)
    {
        if(c.gameObject.CompareTag("Player"))
        {
            if (can_damage)
                StartCoroutine(Damage_Player(c.gameObject));

        }
    }

    private IEnumerator Damage_Player(GameObject player)
    {
        can_damage = false;
        player.GetComponent<Player>().TakeDamage(damage);
        yield return new WaitForSeconds(reloadTime);
        can_damage = true;
    }
}
