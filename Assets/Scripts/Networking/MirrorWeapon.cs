using UnityEngine;
using Mirror;

public class MirrorWeapon : NetworkBehaviour
{
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float projectileSpeed = 12f;
    public float fireRate = 0.2f;

    private float lastFireTime;

    public void Fire(Vector2 direction, PlayerController owner)
    {
        if (!isLocalPlayer) return;
        if (Time.time - lastFireTime < fireRate) return;
        lastFireTime = Time.time;

        if (firePoint == null || projectilePrefab == null) return;

        CmdSpawnProjectile(direction.normalized * projectileSpeed, firePoint.position, owner.transform.position);
    }

    [Command]
    private void CmdSpawnProjectile(Vector2 velocity, Vector3 spawnPos, Vector3 ownerPos)
    {
        GameObject go = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        MirrorProjectile p = go.GetComponent<MirrorProjectile>();
        if (p != null)
        {
            p.SetVelocity(velocity, ownerPos);
        }

        NetworkServer.Spawn(go);
    }
}
