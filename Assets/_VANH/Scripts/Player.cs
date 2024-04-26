using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.Mathematics;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 350;

    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject attackArea;

    [SerializeField] private Transform aTeleport, bTeleport;
    
    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isAttack = false;
    private float horizontal;
    private bool isDeath = false;
    private int coin = 0;
    private Vector3 savePoint;
    private int spikeDamage = 10;
    private float timeToHeal = 10f;
    private float lastActionTime;
    private float healValue = 10f;

    void Awake()
    {
        coin = PlayerPrefs.GetInt("coin", 0);
        lastActionTime = Time.time;
    }
    void Update()
    {
        if (IsDead)
        {
            return;
        }
        isGrounded = CheckGrounded();
        horizontal = Input.GetAxisRaw("Horizontal");
    
        if (isAttack)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        
        if (isGrounded)
        {
            if (isJumping)
            {
                return;
            }
            //jump  
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }
            //change anim run
            if (Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnim("run");
            }
            //attack
            if (Input.GetKeyDown(KeyCode.C) && isGrounded)
            {
                Attack();
            }
            //throw
            if (Input.GetKeyDown(KeyCode.V) && isGrounded)
            {
                Throw();
            }
        }   
        //check falling
        if (!isGrounded && rb.velocity.y < 0)
        {
            ChangeAnim("fall");
            isJumping = false;
        }   

        //Moving
        if (Mathf.Abs(horizontal) > 0.1f)
        {   
            ChangeAnim("run");
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            if (Input.GetKey(KeyCode.R))
            {
                rb.velocity += new Vector2(horizontal * speed, rb.velocity.y);
            }

            if (Input.GetKeyUp(KeyCode.R))
            {
                rb.velocity -= new Vector2(horizontal * speed, rb.velocity.y);
            }
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
        }
        else if (isGrounded)
        {
            ChangeAnim("idle");
            rb.velocity = Vector2.zero;
        }
        //hoi mau khi ko lam j trong 10s
        CheckToHeal();
    }

    public override void OnInit()
    {
        base.OnInit();
        isAttack = false;
        transform.position = savePoint;
        ChangeAnim("idle");
        DeActiveAttack();
        SavePoint();
        UIManager.instance.SetCoin(coin);
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        OnInit();
    }

    protected override void OnDeath()
    {
        base.OnDeath();
    }

    private bool CheckGrounded()
    {
        // Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        return hit.collider != null;
    }

    public void Attack()
    {
        ChangeAnim("attack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);
    }

    public void Throw()
    {
        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
        Instantiate(kunaiPrefab, throwPoint.position, throwPoint.rotation);
    }

    private void ResetAttack()
    {
        isAttack = false;
        // ChangeAnim("ilde");
    }

    public void Jump()
    {
        isJumping = true;
        ChangeAnim("jump");
        if (isGrounded)
        {
            rb.AddForce(jumpForce * Vector2.up);
        }
    }

    internal void SavePoint()
    {
        savePoint = transform.position;
    }

    private void ActiveAttack()
    {
        attackArea.SetActive(true);
    }

    private void DeActiveAttack()
    {
        attackArea.SetActive(false);
    }

    public void SetMove(float horizontal)
    {
        this.horizontal = horizontal;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin")
        {
            coin++;
            PlayerPrefs.SetInt("coin", coin);
            UIManager.instance.SetCoin(coin);
            Destroy(collision.gameObject);
        }

        if (collision.tag == "Deadzone")
        {
            ChangeAnim("die");
            Invoke(nameof(OnInit), 1f);
        }

        if (collision.tag == "Spike")
        {
            OnHit(spikeDamage);
        }

        if (collision.tag == "aTeleport")
        {
            transform.position = bTeleport.position;
        }
    }

    private void CheckToHeal()
    {
        if (Input.anyKey)
        {
            lastActionTime = Time.time;
        }
        else
        {
            float idleTime = Time.time - lastActionTime;
            if (idleTime >= timeToHeal)
            {
                
            }
        }
    }
    
}
