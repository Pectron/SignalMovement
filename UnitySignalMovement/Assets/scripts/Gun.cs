using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private float speed = 100;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private Transform barrel;


    [SerializeField]
    private AudioSource ShootSound, HitSound;

    private float timeWithoutFire;

    private PhysiologySignalsManager _bioGadget;

    private void Awake()
    {
        var bio = GameObject.Find("Biosignals");
        if (bio != null)
        {
            _bioGadget = bio.GetComponent<PhysiologySignalsManager>();
        }
    }

    void Update()
    {
        if(timeWithoutFire > 0)
        {
            timeWithoutFire -= Time.deltaTime;
        }
    }


    public void Fire()
    {
        if (timeWithoutFire > 0)
            return;

        GameObject spawnBullet = Instantiate(bullet, barrel.position, barrel.rotation);
        spawnBullet.GetComponent<Rigidbody>().velocity = speed * barrel.forward;

        ShootSound.volume = Random.Range(0.23f, 0.35f);
        ShootSound.pitch  = Random.Range(0.8f, 1.2f);
        ShootSound.Play();

        Destroy(spawnBullet, 2);
        timeWithoutFire = 0.26f;

        //tiro mandar sinal
        if (_bioGadget != null)
            _bioGadget.NewMarker("clicou no TriggerButton (disparo)");
    }

    public void Hit()
    {
        HitSound.volume = Random.Range(0.2f, 0.3f);
        HitSound.pitch  = Random.Range(0.8f, 1.2f);
        HitSound.Play();
    }
}
