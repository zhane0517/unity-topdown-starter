using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Top-down player movement + dash + shoot hooks.
/// Designed for singleplayer now but contains simple network hooks (SetLocalPlayer, OnNetworkSpawn).
/// Requires: Rigidbody2D, Collider2D. Add Unity's PlayerInput component with "Send Messages" behavior.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float dashSpeed = 12f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 1f;

    [Header("References")]
    public Weapon weapon;

    Rigidbody2D rb;
    Vector2 moveInput;
    bool isDashing;
    bool canDash = true;

    // Network flag - set by network system on spawn
    [HideInInspector] public bool isLocalPlayer = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer) return; // networked clients should be controlled locally only for their player

        if (isDashing) return;
        Vector2 vel = moveInput * moveSpeed;
        rb.velocity = vel;
        // rotate sprite to face movement / aim if desired
        if (vel.sqrMagnitude > 0.01f)
            transform.up = vel.normalized;
    }

    // Input System Send Messages callbacks
    // PlayerInput must be configured to call "OnMove", "OnFire", "OnDash"
    void OnMove(InputValue v)
    {
        moveInput = v.Get<Vector2>();
    }

    void OnFire()
    {
        if (!isLocalPlayer) return;
        if (weapon != null)
        {
            Vector2 aim = moveInput.sqrMagnitude > 0.01f ? moveInput.normalized : (Vector2)transform.up;
            weapon.Fire(aim, this);
        }
    }

    void OnDash()
    {
        if (!isLocalPlayer || !canDash) return;
        StartCoroutine(DashRoutine());
    }

    IEnumerator DashRoutine()
    {
        canDash = false;
        isDashing = true;
        Vector2 dir = moveInput.sqrMagnitude > 0.01f ? moveInput.normalized : transform.up;
        rb.velocity = dir * dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    // Health wrapper - used by projectiles/enemies
    public void ApplyDamage(int amount)
    {
        var h = GetComponent<Health>();
        if (h != null) h.TakeDamage(amount);
    }

    // --- Network hooks / extension points ---
    // Call this from your network spawn to mark authority
    public virtual void SetLocalPlayer(bool value)
    {
        isLocalPlayer = value;
        // e.g., enable camera follow only for local player
    }

    // Called when network layer spawns this object
    public virtual void OnNetworkSpawn()
    {
        // network-specific init
    }
}
