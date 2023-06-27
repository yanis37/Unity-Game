using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public BoxCollider2D boxCollider;
    private Rigidbody2D rb;

    public int damageOnCollision = 20;
    private bool canHit = true; // Flag to track if the enemy can hit the player

    private float ypos;
    private bool isNegativeY;

    private float gravity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        if (gameObject.name == "Ghost(Clone)") // need to modify this later to be more general
        {
            gravity = 0f;
        }
        else
        {
            gravity = 1f;
        }

        ypos = rb.position.y;

        if (ypos < 0)
        {
            rb.gravityScale = -gravity;
            transform.eulerAngles = new Vector3(0, 180, 180);

            isNegativeY = true;
        }
        else
        {
            rb.gravityScale = gravity;
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

        if (ypos < 0 && !isNegativeY)
        {
            rb.gravityScale = -gravity;
            transform.eulerAngles = new Vector3(0, 180, 180);
            isNegativeY = true;

        }
        else if (ypos >= 0 && isNegativeY)
        {
            rb.gravityScale = gravity;
            transform.eulerAngles = new Vector3(0, 0, 0);
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
