using UnityEngine;
using Mirror;

public class NetworkHealth : NetworkBehaviour
{
    public int maxHealth = 5;
    [SyncVar] public int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (!isServer) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            NetworkServer.Destroy(gameObject);
        }
    }
}
