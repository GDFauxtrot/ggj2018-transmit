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
    public GameObject shoot_particles;

    public float cameraFollowStep;
    private AudioSource shoot_sound;

    GameObject cameraFollow;

    private Animator anim;
    public SpriteRenderer sr;

    void Awake() {
        anim = GetComponent<Animator>();
        reticle = transform.GetChild(0).GetComponent<Transform>();
        rb2D = GetComponent<Rigidbody2D>();
        cameraFollow = transform.GetChild(1).gameObject;
        shoot_sound = GetComponent<AudioSource>();
    }

    void Update () {
        if (Input.GetButtonDown("Fire1")) {
            poolapi.request(shoot_particles.transform.position, Quaternion.Euler(0, 0, reticle.rotation.eulerAngles.z),false);
            if (sr.flipX)
            {
                shoot_particles.transform.rotation = Quaternion.Euler(new Vector3(0, 180, -25f));
                shoot_particles.transform.localPosition = new Vector3(-0.6f, 2.1f, 0);
            }
            else
            {
                shoot_particles.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -25f));
                shoot_particles.transform.localPosition = new Vector3(0.6f, 2.1f, 0);
            }
            shoot_particles.GetComponent<ParticleSystem>().Play();
            shoot_sound.pitch = Random.Range(0.9f, 1.1f);
            shoot_sound.Play();
            StartCoroutine(ScreenShake());
        }
    }

    private IEnumerator ScreenShake()
    {
        for(int i = 0; i < 2; ++i)
        {
            cameraFollow.transform.position += new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void TakeDamage(int damage) {
        if (isInvincible && damage < health && damage > 0)
            return;

        health -= damage;
        if (health <= 0) {
            // die function
        }
        health = Mathf.Clamp(health, 0, maxHealth);
        if(damage > 0)
            StartCoroutine(IFrames());
    }

    private IEnumerator IFrames()
    {
        isInvincible = true;
        for(int i = 0; i < 15; ++i)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f);
            yield return new WaitForSeconds(0.05f);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
            yield return new WaitForSeconds(0.05f);
        }
        isInvincible = false;
    }

    void FixedUpdate() {
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (movement.x != 0 || movement.y != 0)
        {
            anim.SetBool("Running", true);
        }
        else
            anim.SetBool("Running", false);


        float right_or_left = Input.GetAxis("RstickHorizontal");
        if (right_or_left != 0)
        {
            if (right_or_left < -0.2f)
                sr.flipX = true;
            else if (right_or_left > 0.2f)
                sr.flipX = false;
        }


        float mag = movement.magnitude;
        movement.Normalize();
        if (mag < 1f)
            movement = new Vector2(movement.x * mag, movement.y * mag);
        
        rb2D.MovePosition(rb2D.position+movement*speed*Time.deltaTime);
        // Mathf.Atan2 returns the tangent line to the two float values given, and then we multiple it to get it as an angle.
        if(new Vector2(Input.GetAxis("RstickVertical"), Input.GetAxis("RstickHorizontal"))!=new Vector2(0,0))
            reticle.eulerAngles = new Vector3(0, 0, Mathf.Atan2(Input.GetAxis("RstickVertical"), Input.GetAxis("RstickHorizontal")) * 180 / Mathf.PI);

        Vector3 prevPosition = transform.position;
        Vector3 nextPosition = (rb2D.position+movement*speed*Time.deltaTime);

        Vector3 delta = (nextPosition - prevPosition) * 5;

        cameraFollow.transform.localPosition = new Vector3(
            Mathf.Lerp(cameraFollow.transform.localPosition.x, delta.x, cameraFollowStep),
            Mathf.Lerp(cameraFollow.transform.localPosition.y + 0.5f, delta.y, cameraFollowStep),
            cameraFollow.transform.position.z);
    }
}
