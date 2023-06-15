using System.Net.Security;
using UnityEditorInternal;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;

    public float moveSpeed;
    public float jumpForce;

    public bool isGrounded = true;
    public bool isJumping;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask collisionLayer;

    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Transform attackPoint;

    private Vector3 velocity = Vector3.zero;
    private float horizontalMovement;
    private CapsuleCollider2D capsuleCollider;


    float ypos;
    bool isNegativeY = false;
    float baseMassPlayer;
    float baseMoveSpeed;




    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        ypos = groundCheck.position.y;

        //define isNegativeY
        if (ypos < 0)
        {
            isNegativeY = true;
        }
        else
        {
            isNegativeY = false;
        }

        baseMassPlayer = rb.mass;
        baseMoveSpeed = moveSpeed;
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionLayer);

        ypos = groundCheck.position.y;
           

        // if (ypos < 0) reverse gravity
         if (ypos < 0 && !isNegativeY)
         {
            rb.gravityScale = -0.01f;
            transform.eulerAngles = new Vector3(0, 180, 180);
            rb.AddForce(new Vector2(0, -1f), ForceMode2D.Impulse); 
            groundCheck.position = new Vector3(groundCheck.position.x, rb.position.y + 0.78f, groundCheck.position.z);
            isNegativeY = true;

        }
         else if (ypos >= 0 && isNegativeY)
         {
            rb.gravityScale = 1f;
            transform.eulerAngles = new Vector3(0, 0, 0);
            rb.AddForce(new Vector2(0, 1f), ForceMode2D.Impulse); 
            groundCheck.position = new Vector3(groundCheck.position.x, rb.position.y - 0.78f, groundCheck.position.z);
            isNegativeY = false;

        }
        

        if (Input.GetButtonDown("Jump") && isGrounded == true)
        {
            isJumping = true;
            animator.SetBool("Jumping", isJumping);
        } 
        else if (isGrounded == true)
        {
            animator.SetBool("Jumping", isJumping);
        }

        //do not flip if keys not pressed
        if (Input.GetAxis("Horizontal") > 0.1f || Input.GetAxis("Horizontal") < -0.1f)
        {
            Flip(rb.velocity.x);
        }



        GetDirection();

        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }

    void FixedUpdate()
    {
        horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        MovePlayer(horizontalMovement);

        if (transform.position.y < 0)
        {
            rb.AddForce(Vector2.up * Physics2D.gravity.magnitude, ForceMode2D.Force);
        } 

    }

    void MovePlayer(float _horizontalMovement)
    {
        Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);

        if (isJumping == true)
        {
            if (ypos < 0)
            {
                rb.AddForce(new Vector2(0f, -jumpForce));
            }
            else
            {
                rb.AddForce(new Vector2(0f, jumpForce));
            }
            isJumping = false;
        }
    }

    void Flip(float _velocity)
    {
        if(_velocity > 0.1f)
        {
            spriteRenderer.flipX = false;
            //also reverse the attack point
            attackPoint.localPosition = new Vector3(0.30f, 0f, 0f);
        } 
        else if(_velocity < -0.1f)
        {
            spriteRenderer.flipX = true;
            //also reverse the attack point
            attackPoint.localPosition = new Vector3(-0.30f, 0f, 0f);
        }
    }

    //method to set speed player to 0
    public void SetHeavyMass()
    {
        rb.mass = 10;
    }

    public void ResetMass()
    {
        rb.mass = baseMassPlayer;
    }

    public bool GetIsGrounded()
    {
        return isGrounded;
    }

    public void ResetMovementSpeed()
    {
        moveSpeed = baseMoveSpeed;
    }

    public void SetMovementSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public bool GetDirection()
    {
        return spriteRenderer.flipX;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

}
