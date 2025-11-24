using UnityEngine;

public class RifleShooter : MonoBehaviour
{
    [Header("Bullet Setup")]
    public Transform muzzlePoint;       // spawn point (child of rifle)
    public GameObject bulletPrefab;

    [Header("Fire Settings")]
    public float fireRate = 0.2f;       // seconds between shots
    public KeyCode fireKey = KeyCode.Mouse0;

    private float nextFireTime = 0f;

    void Update()
    {
        if (Input.GetKey(fireKey) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        if (muzzlePoint == null || bulletPrefab == null)
        {
            Debug.LogWarning("RifleShooter: MuzzlePoint or BulletPrefab not set!");
            return;
        }

        // Spawn bullet at muzzle position & rotation
        GameObject bulletObj = Instantiate(
            bulletPrefab, 
            muzzlePoint.position, 
            muzzlePoint.rotation
        );

        // (Optional) you can tweak bullet settings here if needed:
        // Bullet bullet = bulletObj.GetComponent<Bullet>();
        // bullet.damage = 40f;
    }
}

