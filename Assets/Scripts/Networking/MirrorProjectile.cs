using UnityEngine;
using Mirror;

public class MirrorProjectile : NetworkBehaviour
{
    public int damage = 1;
    public float lifeTime = 3f;

    private Rigidbody2D rb;
    private Vector2 velocity;
    private GameObject owner;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetVelocity(Vector2 v, Vector3 ownerPos)
    {
        velocity = v;
        if (rb != null)
            rb.velocity = velocity;

        if (velocity.sqrMagnitude > 0.01f)
            transform.up = velocity.normalized;

        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == owner) return;

        Health health = other.GetComponent<Health>();
        if (health != null)
            health.TakeDamage(damage);

        NetworkServer.Destroy(gameObject);
    }
}
