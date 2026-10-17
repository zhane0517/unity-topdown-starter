using System.Collections;
using UnityEngine;
using Mirror;

public class MirrorPlayerController : NetworkBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float dashSpeed = 12f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 1f;

    [Header("References")]
    public Weapon weapon;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isDashing;
    private bool canDash = true;

    public override void OnStartLocalPlayer()
    {
        // Local camera attach
        Camera cam = Camera.main;
        if (cam != null)
        {
            cam.transform.SetParent(transform);
            cam.transform.localPosition = new Vector3(0, 0, -10);
        }

        // can also enable HUD
        enabled = true;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        if (isDashing)
            return;

        Vector2 velocity = moveInput * moveSpeed;
        rb.velocity = velocity;

        if (velocity.sqrMagnitude > 0.01f)
        {
            transform.up = velocity.normalized;
        }
    }

    public void OnMove(InputValue value)
    {
        if (!isLocalPlayer) return;
        moveInput = value.Get<Vector2>();
    }

    public void OnFire()
    {
        if (!isLocalPlayer) return;
        if (weapon != null)
        {
            Vector2 aim = moveInput.sqrMagnitude > 0.01f ? moveInput.normalized : (Vector2)transform.up;
            weapon.Fire(aim, this);
        }
    }

    public void OnDash()
    {
        if (!isLocalPlayer || !canDash) return;
        StartCoroutine(DashRoutine());
    }

    private IEnumerator DashRoutine()
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

    public void ApplyDamage(int amount)
    {
        Health h = GetComponent<Health>();
        if (h != null)
            h.TakeDamage(amount);
    }

    // Multiplayer hook
    public void SetLocalPlayer(bool value)
    {
        if (!isLocalPlayer && value)
        {
            // this is only for local authority assignment
        }
    }
}
