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
    public AudioSource hurt;

    GameObject cameraFollow;

    private Animator anim;
    public SpriteRenderer sr;

    public GameObject game_over, eat;

    void Awake() {

        anim = GetComponent<Animator>();
        reticle = transform.GetChild(0).GetComponent<Transform>();
        rb2D = GetComponent<Rigidbody2D>();
        cameraFollow = transform.GetChild(1).gameObject;
        shoot_sound = GetComponent<AudioSource>();
    }

    void Update () {
        if (Input.GetButtonDown("Fire1")) {
            poolapi.request(shoot_particles.transform.position, Quaternion.Euler(0, 0, reticle.rotation.eulerAngles.z),"player");
            if (anim.GetInteger("Direction") == 0)
            {
                shoot_particles.transform.rotation = Quaternion.Euler(new Vector3(0, 180, -25f));
                shoot_particles.transform.localPosition = new Vector3(-0.6f, 2.1f, 0);
            }
            else if(anim.GetInteger("Direction") == 1)
            {
                shoot_particles.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 65f));
                shoot_particles.transform.localPosition = new Vector3(-0.1f, 2.5f, 0);
            }
            else if(anim.GetInteger("Direction") == 3)
            {
                shoot_particles.transform.rotation = Quaternion.Euler(new Vector3(180, 0, 65f));
                shoot_particles.transform.localPosition = new Vector3(0.1f, 1.44f, 0);
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
            Camera.main.GetComponent<AudioListener>().enabled = false;
            game_over.SetActive(true);
            Destroy(gameObject);
            // die function
        }
        health = Mathf.Clamp(health, 0, maxHealth);
        if(damage > 0)
        {
            StartCoroutine(IFrames());
            hurt.Play();
        }
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


        Vector2 rstick = new Vector2(Input.GetAxis("RstickHorizontal"), Input.GetAxis("RstickVertical"));
        anim.SetFloat("SpeedMult", 1);
        bool x_greater = Mathf.Abs(movement.x) > Mathf.Abs(movement.y);
        bool rstick_x_greater = Mathf.Abs(rstick.x) > Mathf.Abs(rstick.y);

        if (rstick != Vector2.zero)
        {
            //if (rstick.x < -0.2f)
            //{
            //    sr.flipX = true;
            //    //Back Running
            //    if (movement.x > 0 && rstick_x_greater)
            //        anim.SetFloat("SpeedMult", -1);
            //}
            //else if (rstick.x > 0.2f)
            //{
            //    sr.flipX = false;
            //    if (movement.x < 0)
            //        anim.SetFloat("SpeedMult", -1);
            //}

            if(rstick_x_greater)
            {
                if(rstick.x > 0.2f)
                {
                    sr.flipX = false;
                    anim.SetInteger("Direction", 2);
                    if (movement.x < 0 && x_greater)
                        anim.SetFloat("SpeedMult", -1);
                }
                else if(rstick.x < -0.2f)
                {
                    sr.flipX = true;
                    anim.SetInteger("Direction", 0);
                    if (movement.x > 0 && x_greater)
                        anim.SetFloat("SpeedMult", -1);
                }
            }
            else
            {
                if(rstick.y > 0.2f)
                {
                    anim.SetInteger("Direction", 1);
                    if (movement.y < 0 && !x_greater)
                        anim.SetFloat("SpeedMult", -1);
                }
                else if(rstick.y < -0.2f)
                {
                    anim.SetInteger("Direction", 3);
                    if (movement.y > 0 && !x_greater)
                        anim.SetFloat("SpeedMult", -1);
                }
            }


            //if(x_greater)
            //{
            //    if (movement.x > 0)
            //            anim.SetInteger("Direction", 2);
            //    else if (movement.x < 0)
            //            anim.SetInteger("Direction", 0);
            //}
            //else
            //{
            //    if (movement.y > 0)
            //        anim.SetInteger("Direction", 1);
            //    else if (movement.y < 0)
            //        anim.SetInteger("Direction", 3);
            //}
        }
        else if (movement != Vector2.zero)
        {
            if (movement.x > 0)
            {
                if (x_greater)
                    anim.SetInteger("Direction", 2);
                else
                    anim.SetInteger("Direction", movement.y > 0 ? 1 : 3);
                sr.flipX = false;
            }
            else if (movement.x < 0)
            {
                if (x_greater)
                    anim.SetInteger("Direction", 0);
                else
                    anim.SetInteger("Direction", movement.y > 0 ? 1 : 3);
                sr.flipX = true;
            }

            if(x_greater)
            {
                if (movement.x > 0)
                    reticle.eulerAngles = new Vector3(0, 0, 0);
                else
                    reticle.eulerAngles = new Vector3(0, 0, 180);
            }
            else
            {
                if(movement.y > 0)
                    reticle.eulerAngles = new Vector3(0, 0, 90);
                else
                    reticle.eulerAngles = new Vector3(0, 0, 270);
            }
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

    public void Munch()
    {
        eat.GetComponent<AudioSource>().Play();
    }
}
