using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D myRidgidBody;
    private Animator myAnimator;
    public float movementSpeed;

    private bool facingRight;

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatisGround;

    private bool isGrounded;

    private bool jump;

    [SerializeField]
    private float jumpForce;
    // Start is called before the first frame update
    void Start()
    {
        facingRight = true;
        myRidgidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = IsGrounded();
        float horizontal = Input.GetAxis("Horizontal");
        HandleMovement(horizontal);
        HandleInput();
        Flip(horizontal);
    }
    
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }
    }

    private void HandleMovement(float horizontal)
    {
        
        if (isGrounded && jump)
        {
            isGrounded = false;
            myRidgidBody.AddForce(new Vector2(0, jumpForce));
        }
        else
        {
            myRidgidBody.velocity = new Vector2(horizontal * movementSpeed, myRidgidBody.velocity.y);

            myAnimator.SetFloat("speed", Mathf.Abs(horizontal));
        }
        if (!isGrounded)
        {
            myAnimator.SetFloat("jump", Mathf.Abs(jumpForce));
        }
        else
        {
            myAnimator.SetFloat("jump", Mathf.Abs(0));
        }

        ResetValues();
    }

    public void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;

            Vector3 playerScale = transform.localScale;

            playerScale.x *= -1;

            transform.localScale = playerScale;
        }
    }

    private bool IsGrounded()
    {
        if(myRidgidBody.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatisGround);

                for(int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void ResetValues()
    {
        jump = false;
    }
}
