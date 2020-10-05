using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;
    Collider2D playerCollider;
    Collider2D platformCollider;

    private float currBoost = 0.0f;
    bool isGrounded;
    private int airJumpsLeft;
    bool jumped;
    bool dropped;
    bool boosted;

    [SerializeField]
    private bool jumpKeyReset = false;

    [SerializeField]
    float vvErr = 0.5f;

    [SerializeField]
    Transform groundCheck;

    [SerializeField]
    Transform groundCheckL;

    [SerializeField]
    Transform groundCheckR;

    [SerializeField]
    GameObject platforms;

    [SerializeField]
    private float runSpeed = 25;

    [SerializeField]
    private float jumpHeight = 30;

    [SerializeField]
    private float addBoostMount = 25;

    [SerializeField]
    private float boostDecayPerSecond = 5.0f;

    [SerializeField]
    bool autoRun = false;

    [SerializeField]
    bool dblJumpEnabled;

    [SerializeField]
    bool spdBrstEnabled;

    [SerializeField]
    int airjumpCount = 1;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();
        platformCollider = platforms.GetComponent<Collider2D>();
        PlatformEffector2D platformEffector = platformCollider.GetComponent<PlatformEffector2D>();

        //Disable PlatformEffector2D
        platformEffector.useColliderMask = false;
        dblJumpEnabled = false;
        spdBrstEnabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown("space") && !(Input.GetKey("s") || Input.GetKey("down")))
        {
            jumped = true;
        }
        if (Input.GetKeyUp("space"))
        {
            jumped = false;
        }
        if ((Input.GetKey("s") || Input.GetKey("down")) && Input.GetKey("space"))
        {
            dropped = true;
        }
        if ((Input.GetKeyUp("s") || Input.GetKeyUp("down")) || Input.GetKeyUp("space"))
        {
            dropped = false;
        }
        if (Input.GetKeyDown("left shift"))
        {
            boosted = true;
        }
        if (Input.GetKeyUp("left shift"))
        {
            boosted = false;
        }
    }
    private void FixedUpdate()
    {
        if (rb2d.velocity.x > runSpeed)
        {
            boosted = false;
        }

        if ((Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"))) ||
           (Physics2D.Linecast(transform.position, groundCheckL.position, 1 << LayerMask.NameToLayer("Ground"))) ||
           (Physics2D.Linecast(transform.position, groundCheckR.position, 1 << LayerMask.NameToLayer("Ground"))) ||
           (Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Platform"))) ||
           (Physics2D.Linecast(transform.position, groundCheckL.position, 1 << LayerMask.NameToLayer("Platform"))) ||
           (Physics2D.Linecast(transform.position, groundCheckR.position, 1 << LayerMask.NameToLayer("Platform"))))
        {
            isGrounded = true;
            airJumpsLeft = airjumpCount;
        }
        else
        {
            isGrounded = false;
            //animator.Play("Player_jump");
        }

        if (autoRun)
        {
            if (spdBrstEnabled)
            {
                if (boosted)
                {
                    currBoost = currBoost + addBoostMount;
                    boosted = false;
                }
                rb2d.velocity = new Vector2(runSpeed + currBoost, rb2d.velocity.y);
                //if(isGrounded && rb2d.velocity.y >= -vvErr && rb2d.velocity.y < vvErr)
                    //animator.Play("Player_run");
            }
            else
            {
                rb2d.velocity = new Vector2(runSpeed + currBoost, rb2d.velocity.y);
                //if(isGrounded && rb2d.velocity.y >= -vvErr && rb2d.velocity.y < vvErr)
                    //animator.Play("Player_run");
            }
        }
        else
        {
            if (Input.GetKey("d") || Input.GetKey("right"))
            {
                rb2d.velocity = new Vector2(runSpeed + currBoost, rb2d.velocity.y);
                //if(isGrounded && rb2d.velocity.y >= -vvErr && rb2d.velocity.y < vvErr)
                    //animator.Play("Player_run");
                spriteRenderer.flipX = false;
            }
            else if (Input.GetKey("a") || Input.GetKey("left"))
            {
                rb2d.velocity = new Vector2(-runSpeed - currBoost, rb2d.velocity.y);
                //if(isGrounded && rb2d.velocity.y >= -vvErr && rb2d.velocity.y < vvErr)
                    //animator.Play("Player_run");
                spriteRenderer.flipX = true;
            }
            else
            {
                //if(isGrounded && rb2d.velocity.y >= -vvErr && rb2d.velocity.y < vvErr)
                    //animator.Play("Player_idle");
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            }
        }

        if (dblJumpEnabled)
        {
            if (isGrounded)
            {
                if (jumped && rb2d.velocity.y <= vvErr)
                {
                    jumped = false;
                    rb2d.velocity = new Vector2(rb2d.velocity.x, jumpHeight);
                    //animator.Play("Player_jump");
                }
            }
            else
            {
                if (jumped && airJumpsLeft > 0)
                {
                    rb2d.velocity = new Vector2(rb2d.velocity.x, jumpHeight);
                    //animator.Play("Player_jump");
                    airJumpsLeft--;
                }
                    
            }
        }
        else
        {
            if (jumped && isGrounded && rb2d.velocity.y <= vvErr)
            {
                jumped = false;
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpHeight);
                //animator.Play("Player_jump");
            }
        }

        if (dropped && isGrounded)
        {
            StartCoroutine(getDropInput());
        }

        manageBoost();
    }

    private void manageBoost()
    {
        if (currBoost > 0)
            currBoost -= boostDecayPerSecond*Time.deltaTime;
        else
            currBoost = 0.0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Boost"))
        {
            currBoost = addBoostMount;
        }
        if (collision.gameObject.name == "Double_Jump")
        {
            Destroy(collision.gameObject);
            dblJumpEnabled = true;
        }
        if (collision.gameObject.name == "Speed_Burst")
        {
            Destroy(collision.gameObject);
            spdBrstEnabled = true;
        }
    }

    IEnumerator getDropInput()
    {
        Physics2D.IgnoreLayerCollision(9, 10, true);
        //animator.Play("Player_jump");
        yield return new WaitForSeconds(0.4f);
        Physics2D.IgnoreLayerCollision(9, 10, false);
    }
}