using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Bullet : MonoBehaviour
{
    public LayerMask damageable;
    /*
    public ParticleSystem particles;
    public AudioSource explosion;
    */
    public float maxDmg = 100f;
    public float force = 1000f;
    public float lifeTime = 2f;
    public float explodeRad = 5f;

    // Setup the destroy object timer
    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // If this object overlaps another 
    // apply explosion force
    // apply damage to the reciever if it has tank health
    // Destroy this object early
    private void OnTriggerEnter(Collider other)
    {
        Collider[] col = Physics.OverlapSphere(transform.position, explodeRad, damageable);

        for(int i = 0; i < col.Length; i++)
        {
            Rigidbody targetBod = col[i].GetComponent<Rigidbody>();

            if (!targetBod) continue;
            targetBod.AddExplosionForce(force, transform.position, explodeRad);

            TankHealth targetHP = targetBod.GetComponent<TankHealth>();

            if (!targetHP) continue;

            float dmg = CalcDmg(targetBod.position);
            targetHP.TakeDamage(dmg);
        }
        /*
        particles.transform.parent = null;
        particles.Play();
        explosion.Play();
        Destroy(particles.gameObject, particles.duration);
        */
        Destroy(gameObject);
    }

    // Calculate damage based on distance
    private float CalcDmg(Vector3 targetPos)
    {
        Vector3 explodeToTarget = targetPos - transform.position;
        float distance = explodeToTarget.magnitude;
        float relativeDis = (explodeRad - distance) / explodeRad;
        float damage = relativeDis * maxDmg;
        damage = Mathf.Max(0f, damage);
        return damage;
    }
}
