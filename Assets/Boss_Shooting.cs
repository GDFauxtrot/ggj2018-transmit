using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Shooting : MonoBehaviour {

    public BulletPoolScriptable bulletPool;
    public GameObject left, right, death_scream;

    public AudioClip shoot, small_explode, big_explode;
    private AudioSource sounds;

    private bool can_shoot = true;

    public GameObject explosion_small, explosion_large;
    public GameObject floating;

    // Use this for initialization
    void Start () {
        sounds = GetComponent<AudioSource>();
        StartCoroutine(ShootEverywhere());
	}

    private IEnumerator ShootEverywhere()
    {
        while(can_shoot)
        {
            for(int i = 0; i < 20; ++i)
            {
                bool l = Random.Range(0, 2) == 0 ? true : false;

                Vector3 new_pos = l ? left.transform.position + new Vector3(Random.Range(-1, -5f), Random.Range(-1f, 1f), 0) : right.transform.position + new Vector3(Random.Range(1, 5f), Random.Range(-1f, 1f), 0);
                new_pos = new_pos - (l ? left.transform.position : right.transform.position);
                Quaternion angle = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(new_pos.y, new_pos.x) * Mathf.Rad2Deg));

                sounds.clip = shoot;
                sounds.pitch = Random.Range(0.8f, 1.3f);
                sounds.volume = 0.5f;
                sounds.Play();
                bulletPool.request(l ? left.transform.position : right.transform.position, angle, "boss");
                yield return new WaitForSeconds(0.1f);
            }
            //shoots
            yield return new WaitForSeconds(Random.Range(1f, 2f));
        }
    }

    public IEnumerator Death()
    {
        sounds.volume = 1;
        can_shoot = false;
        death_scream.SetActive(true);
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 4; ++i)
        {
            sounds.clip = small_explode;
            sounds.pitch = Random.Range(0.9f, 1.1f);
            sounds.Play();
            Instantiate(explosion_small, transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0), Quaternion.identity);
            yield return new WaitForSeconds(0.145f);
            //play small explosions
        }
        yield return new WaitForSeconds(1);
        sounds.clip = big_explode;
        sounds.Play();
        Instantiate(explosion_large, transform.position, Quaternion.identity);
        GetComponent<SpriteRenderer>().enabled = false;
        floating.SetActive(false);
        yield return new WaitForSeconds(2f);
        PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + 500);
        Destroy(gameObject);
    }

    private void delete()
    {
        Destroy(gameObject);
    }
	
}
