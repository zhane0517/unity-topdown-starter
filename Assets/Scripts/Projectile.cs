using UnityEngine;

/// <summary>
/// Projectile movement and simple damage-on-hit.
/// Requires Rigidbody2D (set to Kinematic or Dynamic depending on collision).
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public int damage = 1;
    public float lifeTime = 3f;
    Rigidbody2D rb;
    PlayerController owner;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Setup(Vector2 velocity, PlayerController ownerController)
    {
        rb.velocity = velocity;
        owner = ownerController;
        Destroy(gameObject, lifeTime);
        if (velocity.sqrMagnitude > 0.01f)
            transform.up = velocity.normalized;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == owner?.gameObject) return; // don't hit owner

        var health = other.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }

        // TODO: spawn impact VFX
        Destroy(gameObject);
    }
}
