using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAttack : MonoBehaviour
{
    public float attackRange = 2f;
    public Transform target;
    public Animator animator;
    private Vector2 initialPosition;

    float distanceToTarget;

    private float attackCooldown = 2f; // The cooldown time in seconds
    private float attackTimer = 0f; // Timer to track the cooldown

    public float teleportDistance = 5f;
    public float minTeleportDistance = 3f;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponentInChildren<Animator>();
        initialPosition = transform.position;

    }

    void Update()
    {
        // Check if the player is within attack range
        distanceToTarget = Vector2.Distance(transform.position, target.position);

        if (distanceToTarget <= attackRange)
        {
            if (attackTimer <= 0f)
            {     
                animator.SetTrigger("IsAttacking");
                attackTimer = attackCooldown; // Reset the cooldown timer
            }
            else
            {
                attackTimer -= Time.deltaTime; // Reduce the cooldown timer
            }
        }
        else
        {
            attackTimer = 0f; // Reset the cooldown timer if the player is out of range
        }
    }

    void Attack()
    {
        Debug.Log("Attacking the player!");
    }

   

}
