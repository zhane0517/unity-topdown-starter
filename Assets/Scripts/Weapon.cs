using UnityEngine;

/// <summary>
/// Simple hitscan/projectile weapon wrapper. Fires projectiles from firePoint.
/// Fire method includes a network hook event (OnProjectileFired) for network replication.
/// </summary>
public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float projectileSpeed = 12f;
    public float fireRate = 0.2f; // seconds
    float lastFireTime;

    public delegate void ProjectileFiredDelegate(Vector2 position, Vector2 direction);
    public event ProjectileFiredDelegate OnProjectileFired; // network layer can subscribe to replicate

    public void Fire(Vector2 direction, PlayerController owner)
    {
        if (Time.time - lastFireTime < fireRate) return;
        lastFireTime = Time.time;

        // local spawn
        var go = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        var pj = go.GetComponent<Projectile>();
        if (pj != null) pj.Setup(direction.normalized * projectileSpeed, owner);

        // network replication hook - network system should listen and send spawn RPC
        OnProjectileFired?.Invoke(firePoint.position, direction.normalized);
    }
}
