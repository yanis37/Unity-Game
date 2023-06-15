using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float speed;
    public Transform[] waypoints;

    public SpriteRenderer spriteRenderer;

    private Transform target;
    private int destPoint;

    private int damageOnCollision = 20;
    private bool canHit = true; // Flag to track if the enemy can hit the player

    private float ypos;
    private bool isNegativeY;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        target = waypoints[0];

        rb = GetComponent<Rigidbody2D>();

        ypos = rb.position.y;

        if (ypos < 0)
        {
            rb.gravityScale = -1f;
            transform.eulerAngles = new Vector3(0, 180, 180);

            isNegativeY = true;
        }
        else
        {
            rb.gravityScale = 1f;
            transform.eulerAngles = new Vector3(0, 0, 0);

            isNegativeY = false;
        }

        //force layer to be enemy
        gameObject.layer = 7;
    }




    public IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        ypos = rb.position.y;

        //Debug.Log(ypos);

        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) < 0.3f)
        {
            destPoint = (destPoint + 1) % waypoints.Length;
            target = waypoints[destPoint];
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        if (ypos < 0 && !isNegativeY)
        {
            rb.gravityScale = -1f;
            transform.eulerAngles = new Vector3(0, 180, 180);
            Debug.Log("swith down");
            //rb.AddForce(new Vector2(0, -1f), ForceMode2D.Impulse);
            isNegativeY = true;

        }
        else if (ypos >= 0 && isNegativeY)
        {
            rb.gravityScale = 1f;
            Debug.Log("swith up");
            transform.eulerAngles = new Vector3(0, 0, 0);
            //rb.AddForce(new Vector2(0, 1f), ForceMode2D.Impulse);
            isNegativeY = false;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canHit)
        {

            if (collision.collider.CompareTag("Shield"))
            {
                // Do nothing if the collision is with the shield
                return;
            }
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damageOnCollision);
            canHit = false;
            StartCoroutine(ResetCanHit());
        }     
    }

    IEnumerator ResetCanHit()
    {
        yield return new WaitForSeconds(1f);
        canHit = true;
    }

}
