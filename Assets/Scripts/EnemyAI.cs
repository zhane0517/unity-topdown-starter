using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Very simple state-based enemy: Patrol -> Chase -> Attack (attack is just damage on contact).
/// Designed to be easy to replace with a more advanced pathfinding system later.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    public enum State { Patrol, Chase }
    public State state = State.Patrol;

    [Header("Patrol")]
    public List<Transform> patrolPoints;
    public float patrolSpeed = 2f;
    int patrolIndex = 0;

    [Header("Chase")]
    public float chaseSpeed = 3.5f;
    public float detectionRadius = 5f;
    public float loseSightRadius = 7f;
    public LayerMask playerMask;

    Rigidbody2D rb;
    Transform targetPlayer;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void Update()
    {
        DetectPlayer();

        switch (state)
        {
            case State.Patrol:
                PatrolUpdate();
                break;
            case State.Chase:
                ChaseUpdate();
                break;
        }
    }

    void DetectPlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, playerMask);
        if (hit)
        {
            targetPlayer = hit.transform;
            state = State.Chase;
        }
        else if (state == State.Chase)
        {
            if (Vector2.Distance(transform.position, targetPlayer?.position ?? transform.position) > loseSightRadius)
            {
                targetPlayer = null;
                state = State.Patrol;
            }
        }
    }

    void PatrolUpdate()
    {
        if (patrolPoints == null || patrolPoints.Count == 0) return;
        Transform dest = patrolPoints[patrolIndex];
        Vector2 dir = (dest.position - transform.position).normalized;
        rb.velocity = dir * patrolSpeed;
        if (Vector2.Distance(transform.position, dest.position) < 0.2f)
        {
            patrolIndex = (patrolIndex + 1) % patrolPoints.Count;
        }
    }

    void ChaseUpdate()
    {
        if (targetPlayer == null)
        {
            state = State.Patrol;
            return;
        }
        Vector2 dir = (targetPlayer.position - transform.position).normalized;
        rb.velocity = dir * chaseSpeed;
        transform.up = dir;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, loseSightRadius);
    }
}
