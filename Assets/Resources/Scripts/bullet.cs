using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour {


	private string tagTarget;
	public float playerSpeed;
	public float enemyspeed;
	public float playerDamage;
	public float enemyDamage;
	private float speed;
	private float damage;
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

	public void ReturnToPool(bool enemy)
	{	
		if(enemy)
		{
			speed=enemyspeed;
			damage=enemyDamage;
			tagTarget="Player";
		}
		else
		{
			speed = playerSpeed;
            damage = playerDamage;
            tagTarget = "Enemy";
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
