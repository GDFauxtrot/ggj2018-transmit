using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour {



 	public bulletScriptable bulletScript;
	private string tagTarget;


	private float speed;
	private int damage;


	private Rigidbody2D rb2d;
	public GameObject pool;


	// Update is called once per frame
	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Awake()
	{
		rb2d=GetComponent<Rigidbody2D>();
        pool = transform.parent.gameObject;
        // transform.rotation=(transform.localRotation);
    }

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>


	void FixedUpdate () {
		Vector2 moving=transform.right*speed*Time.deltaTime;

		rb2d.MovePosition(rb2d.position+moving);
	}


	void OnTriggerEnter2D(Collider2D other)
	{
		if(tagTarget=="Enemy"&& other.tag=="Enemy")
		{
			other.GetComponent<DumbEnemyAI>().enemyHealth-=damage;
			if(bulletScript.upgraded)
			{
				Debug.Log("ITTAI NEEDS TO WRITE THIS NOW");
			}
		}
		if(tagTarget=="Player"&& other.tag=="Player")
		{
			other.GetComponent<Player>().SetHealth(other.GetComponent<Player>().health-damage);
		}
		CancelInvoke();

        RET();
	}
	
	public void ReturnToPool(bool enemy)
	{	
		if(enemy)
		{
			speed=bulletScript.EnemySpeed;
			damage=bulletScript.EnemyDamage;
			tagTarget="Player";
			gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = bulletScript.bulletTypes[2];
		}
		else
		{
			if(bulletScript.upgraded){
				speed = bulletScript.UpPlayerSpeed;
				damage = bulletScript.UpPlayerDamage;
				gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite=bulletScript.bulletTypes[1];
				tagTarget = "Enemy";
			}
			else
			{
                speed = bulletScript.UpPlayerSpeed;
                damage = bulletScript.UpPlayerDamage;
                gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = bulletScript.bulletTypes[0];
                tagTarget = "Enemy";
			}
		}
        gameObject.transform.parent = null;
        Invoke("RET",2);
	}

	void RET()
	{
        transform.position = pool.transform.position;
		gameObject.transform.SetParent(pool.transform);
		gameObject.SetActive(false);
    }
}
