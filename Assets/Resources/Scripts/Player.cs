using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// If this name causes problems, do not hesitate to change it (or ask Greg to)
public class Player : MonoBehaviour {

    public int health;
    public int maxHealth;

    public float speed;

    public bool isInvincible; // Just set this and let Update take care of the rest

    private Rigidbody2D rb2D;
    private Transform reticle;

    public BulletPoolScriptable poolapi;

    public float cameraFollowStep;

    GameObject cameraFollow;

    void Awake() {
        reticle = transform.GetChild(0).GetComponent<Transform>();
        rb2D = GetComponent<Rigidbody2D>();
        cameraFollow = transform.GetChild(1).gameObject;
    }

    void Update () {
        if (Input.GetButtonDown("Fire1")) {
            poolapi.request(reticle.position + (reticle.right), Quaternion.Euler(0, 0, reticle.rotation.eulerAngles.z),false);
        }
    }

    public void SetHealth(int h) {
        if (isInvincible && h < health)
            return;

        health = h;
        if (health <= 0) {
            // die function
        }
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    void FixedUpdate() {
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        float mag = movement.magnitude;
        movement.Normalize();
        if (mag < 1f)
            movement = new Vector2(movement.x * mag, movement.y * mag);
        
        rb2D.MovePosition(rb2D.position+movement*speed*Time.deltaTime);
        // Mathf.Atan2 returns the tangent line to the two float values given, and then we multiple it to get it as an angle.
        
        reticle.eulerAngles = new Vector3(0, 0, Mathf.Atan2(Input.GetAxis("RstickVertical"), Input.GetAxis("RstickHorizontal")) * 180 / Mathf.PI);

        Vector3 prevPosition = transform.position;
        Vector3 nextPosition = (rb2D.position+movement*speed*Time.deltaTime);

        Vector3 delta = nextPosition - prevPosition;

        cameraFollow.transform.localPosition = new Vector3(
            Mathf.Lerp(cameraFollow.transform.localPosition.x, delta.x, cameraFollowStep),
            Mathf.Lerp(cameraFollow.transform.localPosition.y, delta.y, cameraFollowStep),
            cameraFollow.transform.position.z);
    }
}
