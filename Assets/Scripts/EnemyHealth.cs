using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    int currentHealth;

    public float knockbackForce = 3f;
    public Rigidbody2D rb;
    public Transform spawnPoint;

    void Start()
    {
        currentHealth = maxHealth;
        spawnPoint = transform.parent;
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int damage, Transform playerTransform, Vector2 dir)
    {
        currentHealth -= damage;

        StartCoroutine(FlashRed());

        ApplyKnockback(-dir, knockbackForce);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator FlashRed()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
        }
    }

    void Die()
    {
        RandomSpawner spawner = spawnPoint.GetComponentInParent<RandomSpawner>();

        if (spawner != null)
        {
            spawner.EnemyKilled(spawnPoint);
            currentHealth = maxHealth; // Reset health for respawn
        }

        Destroy(transform.parent.gameObject);
    }

    public void SetSpawnPoint(Transform spawn)
    {
        spawnPoint = spawn;
    }

    void ApplyKnockback(Vector2 direction, float force)
    {
        direction.x = direction.x * force;
        rb.AddForce(direction, ForceMode2D.Impulse);
    }
}