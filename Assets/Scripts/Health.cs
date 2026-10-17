using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Simple health component with events. Exposes TakeDamage and Heal.
/// </summary>
public class Health : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;
    public bool invulnerable = false;

    public UnityEvent<int> OnDamaged; // param = damage amount
    public UnityEvent OnDead;
    public UnityEvent<int> OnHealed; // param = amount healed

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (invulnerable) return;
        currentHealth -= amount;
        OnDamaged?.Invoke(amount);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealed?.Invoke(amount);
    }

    void Die()
    {
        OnDead?.Invoke();
        // default destroy; network layer should override behavior
        Destroy(gameObject);
    }
}
