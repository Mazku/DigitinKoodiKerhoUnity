using UnityEngine;
using System.Collections;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : BaseBehaviour
{

    public float maxSpeed = 6f;
    public float jumpForce = 1000f;
    public float verticalSpeed = 20;

    public BoxCollider2D groundCheckCollider;
    public LayerMask whatIsGround;

    public GameObject cloudPrefab;

    // variable rigidbody used to exist in Unity, now is obsolete so use keyword new to inform compiler that you are hiding the superclass:rigidbody on purpose
    new Rigidbody2D rigidbody;
    Animator animator;

    bool lookingRight = true;
    bool isGrounded = false;
    bool doubleJump = false;

    // Called just before first tick
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Called on collission
    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.relativeVelocity.magnitude > 20)
        {
            Instantiate(cloudPrefab, transform.position, transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Handle jump (space or up key)
        if ((Input.GetButtonDown("Jump") || (Input.GetButtonDown("Vertical") && Input.GetAxis("Vertical") > 0))
            && (isGrounded || !doubleJump))
        {
            rigidbody.AddForce(new Vector2(0, jumpForce));

            if (!doubleJump && !isGrounded)
            {
                doubleJump = true;
                Instantiate(cloudPrefab, transform.position, transform.rotation);
            }
        }

        // Handle boost to down (down key)
        if (Input.GetButtonDown("Vertical") && Input.GetAxis("Vertical") < 0 && !isGrounded)
        {
            rigidbody.AddForce(new Vector2(0, -jumpForce));
            Instantiate(cloudPrefab, transform.position, transform.rotation);
        }
    }

    // Update is called once per physics tick
    void FixedUpdate()
    {
        if (isGrounded)
        {
            doubleJump = false;
        }

        float horizontalInput = Input.GetAxis("Horizontal");

        rigidbody.velocity = new Vector2(horizontalInput * maxSpeed, rigidbody.velocity.y);
        isGrounded = Physics2D.OverlapCircle(groundCheckCollider.transform.position, groundCheckCollider.size.y / 2 + 0.15F, whatIsGround);


        // Set looking direction
        if ((horizontalInput > 0 && !lookingRight) || (horizontalInput < 0 && lookingRight))
        {
            var localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;

            lookingRight = !lookingRight;
        } 

        // Set animator parameters
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("horizontalSpeed", GetComponent<Rigidbody2D>().velocity.x);
        animator.SetFloat("verticalSpeed", GetComponent<Rigidbody2D>().velocity.y);
    }

}