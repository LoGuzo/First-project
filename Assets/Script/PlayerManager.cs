using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float playerSpeed;
    public float JumpPower;
    public float wallJumpPower;
    public float m_rollForce = 6.0f;
    public Transform pos;
    public Vector2 boxSize;
    private bool isWallJump;
    private int jumpCount = 0;
    private int m_currentAttack = 0;
    private float m_timeSinceAttack = 0.0f;
    // private int m_facingDirection = 1;

    public Transform wallChk;
    public Transform groundChkFront;
    public Transform groundChkBack;
    
    float input_x;
    public float wallchkDistance;
    public float chkDistance;
    public LayerMask w_Layer;
    public LayerMask g_Layer;

    public GameManager gameManager;
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;

   // bool m_rolling = false;
    bool doubleJump = false;
    bool isGround;
    float isRight=1;
    bool isWall; // º®°¨Áö
    public float slidingSpeed;
    // Start is called before the first frame update

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        m_timeSinceAttack += Time.deltaTime;
        isWall = Physics2D.Raycast(wallChk.position, Vector2.right * isRight, wallchkDistance, w_Layer);
        anim.SetBool("Wall", isWall);
        bool ground_front = Physics2D.Raycast(groundChkFront.position, Vector2.down, chkDistance, g_Layer);
        bool ground_back = Physics2D.Raycast(groundChkBack.position, Vector2.down, chkDistance, g_Layer);
        
        input_x = Input.GetAxis("Horizontal");
        if (ground_front || ground_back)
            isGround = true;
        else
            isGround = false;

        anim.SetBool("isGround", isGround);

        anim.SetFloat("yVelocity", rigid.velocity.y);
        Jump();
        Attack();
        // Roll();

    }
    void FixedUpdate()
    {
        if (!isWallJump)
            rigid.velocity = new Vector2(input_x*playerSpeed, rigid.velocity.y);
        Move();

    }
    void FreezeX()
    {
        isWallJump = false;
    }
    private void Move()
    {
        if (isWallJump == false)
        {
            if (isWall == true)
            {
                anim.SetBool("Wall", false);
                if (isGround == false)
                {
                    anim.SetTrigger("isJumping");
                }
            }

            if ((Input.GetAxis("Horizontal") > 0 && isRight < 0) || (Input.GetAxis("Horizontal") < 0 && isRight > 0))
            {
                FlipPlayer();
                anim.SetBool("isRunning", true);
            }
            else if (Input.GetAxis("Horizontal") == 0)
            {
                anim.SetBool("isRunning", false);
            }
            else
            {
                anim.SetBool("isRunning", true);
            }
        }

    }
    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGround && jumpCount < 2) 
        {
            anim.SetTrigger("isJumping");
            anim.SetBool("isRunning", false);
            isGround = false;
            jumpCount++;
            doubleJump = true;
            rigid.velocity = Vector2.up * JumpPower;
        }
        else if (Input.GetButtonDown("Jump") && doubleJump)
        {
            anim.SetTrigger("isJumping");
            rigid.velocity = rigid.velocity * 0.5f;
            rigid.velocity = Vector2.up * JumpPower;

            doubleJump = false;
        }
        if (isWall==true)
        {
            jumpCount = 0;
            isWallJump = false;
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * slidingSpeed);
            if (Input.GetAxis("Jump") != 0) 
            {
                anim.SetBool("isRunning", false);
                isWallJump = true;
                Invoke("FreezeX", 0.3f);
                anim.SetTrigger("isJumping");
                rigid.velocity = new Vector2(-isRight * wallJumpPower, 0.9f * wallJumpPower);
                FlipPlayer();
            }
        }
        

    }
    void Attack()
    {
        int randomDamge = Random.Range(1, 5);
        if (Input.GetKeyDown(KeyCode.A) && m_timeSinceAttack > 0.25f)
        {
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
            foreach (Collider2D collider in collider2Ds)
            {
                if(collider.tag=="Enemy")
                collider.GetComponent<BossManaager>().EnemyHealthDown(randomDamge);
            }
            m_currentAttack++;

            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            anim.SetTrigger("Attack" + m_currentAttack);

            // Reset timer
            m_timeSinceAttack = 0.0f;
        }
    }
    /*void Roll()
    {
        if (Input.GetKeyDown("left shift") && !m_rolling && !isWall)
        {
            m_rolling = true;
            anim.SetTrigger("Roll");
            rigid.velocity = new Vector2(m_facingDirection * m_rollForce, rigid.velocity.y);
        }
    }*/
    void FlipPlayer()
    {
        transform.eulerAngles = new Vector3(0, Mathf.Abs(transform.eulerAngles.y - 180), 0);
        isRight *= -1;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.7f)
        {
            if (collision.gameObject.layer == 7) 
            {
                isGround = true;
                jumpCount = 0;
            }
        }
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            OnDamaged();
            
        }
 
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(wallChk.position, Vector2.right * isRight * wallchkDistance);
        Gizmos.DrawWireCube(pos.position, boxSize);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(groundChkFront.position, Vector2.down * chkDistance);
        Gizmos.DrawRay(groundChkBack.position, Vector2.down * chkDistance);
    }
    void OnDamaged()
    {
        gameManager.HealthDown();
    }
}
