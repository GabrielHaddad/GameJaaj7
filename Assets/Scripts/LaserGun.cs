using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    [SerializeField] GameObject laserBullet;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileLifetime = 1f;
    [SerializeField] float baseFiringRate = 0.5f;

    void Start()
    {
        StartCoroutine(FireContinuously());
    }

    IEnumerator FireContinuously()
    {
        while (true) 
        {
            GameObject instance = Instantiate(laserBullet, 
                                    transform.position, transform.rotation);
                        
            Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = -transform.up * projectileSpeed;
            }

            Destroy(instance, projectileLifetime);
            
            yield return new WaitForSeconds(baseFiringRate);
        }
    }
}
