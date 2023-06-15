using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 1f;
    public LayerMask enemyLayers;

    public int baseAttackDamage = 40;
    public int attackDamage;
    public float attackRate = 2f;
    float nextAttackTime = 0f;

    public int critRate = 10;
    public Vector2 direction;
    public float knockbackVertical = 2f;

    public Rigidbody2D rb;
    private float ypos;
    private bool yposPositive;

    public bool isShielding = false;
    public bool canAttack = true;

    //get rigedbody2d of the player ont the start

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        ypos = rb.position.y;

        if(ypos < 0)
        {
            yposPositive = false;
        }
        else
        {
            yposPositive = true;
        }
        
    }


    void Update()
    {

        ypos = rb.position.y;

        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !isShielding)
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }

            if (Input.GetMouseButton(1))
            {
                Shield();
            }
            else
            {
                isShielding = false;
                GameObject shieldObject = transform.Find("Shield").gameObject;
                BoxCollider2D shieldCollider = shieldObject.GetComponent<BoxCollider2D>();
                shieldCollider.enabled = false;
                animator.SetBool("Shield", isShielding);
                GetComponent<PlayerMovement>().ResetMass();
                GetComponent<PlayerMovement>().ResetMovementSpeed();
            }
        }  
    }

    void Attack()
    {
        animator.SetTrigger("Attack");

        bool isCriticalHit = Random.Range(0, 100) < critRate;
        if (isCriticalHit) {
            attackDamage = baseAttackDamage * 2;
        } else {
            attackDamage = baseAttackDamage;
        }

        if (yposPositive && ypos < 0)
        {
            knockbackVertical = -knockbackVertical;
            Debug.Log("knockbackVertical: " + knockbackVertical);

            // Set the flag to false to avoid re-entering the if statement
            yposPositive = false;
        }
        else if (!yposPositive && ypos >= 0)
        {
            Debug.Log("knockbackVertical: " + knockbackVertical);
            knockbackVertical = -knockbackVertical;

            // If ypos changes back to positive, reset the flag to true
            yposPositive = true;
        }


        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            direction = new Vector3(rb.position.x - enemy.transform.position.x, -knockbackVertical);
            Debug.Log(direction);
            enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage, attackPoint, direction);
            DamagePopup.Create(enemy.transform.position, attackDamage, isCriticalHit);
        }
    }

    //shielding
    void Shield()
    {
        isShielding = true;
        animator.SetBool("Shield", isShielding);
        //turn on the box collider of the game object named "shield"
        GameObject shieldObject = transform.Find("Shield").gameObject;
        BoxCollider2D shieldCollider = shieldObject.GetComponent<BoxCollider2D>();
        bool direction = GetComponent<PlayerMovement>().GetDirection();
        if(direction)
        {
            //shied left
            shieldObject.transform.localPosition = new Vector3(0.048f, -0.05f, 0);
        }
        else
        {   
            //shield right  
            shieldObject.transform.localPosition = new Vector3(0.5f, -0.05f, 0);
        }

        shieldCollider.enabled = true;


        if (GetComponent<PlayerMovement>().GetIsGrounded())
        {
            GetComponent<PlayerMovement>().SetHeavyMass();
            GetComponent<PlayerMovement>().SetMovementSpeed(10);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

    }

    

}
