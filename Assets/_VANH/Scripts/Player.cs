using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 5f;
    [SerializeField] private Animator anim;
    [SerializeField] private float jumpForce = 350;
    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isAttack = false;
    private float horizontal;
    private string currentAnimName;
    private bool isDeath = false;
    private int coin = 0;

    void FixedUpdate()
    {
        if (isDeath)
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
            rb.velocity = new Vector2(horizontal * Time.fixedDeltaTime * speed, rb.velocity.y);
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
        }
        else if (isGrounded)
        {
            ChangeAnim("idle");
            rb.velocity = Vector2.zero;
        }
    }
    private bool CheckGrounded()
    {
        // Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        return hit.collider != null;
    }

    private void Attack()
    {
        ChangeAnim("attack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
    }

    private void Throw()
    {
        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
    }

    private void ResetAttack()
    {
        isAttack = false;
        // ChangeAnim("ilde");
    }

    private void Jump()
    {
        isJumping = true;
        ChangeAnim("jump");
        rb.AddForce(jumpForce * Vector2.up);
    }

    private void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin")
        {
            coin++;
            Destroy(collision.gameObject);
        }

        if (collision.tag == "Deadzone")
        {
            ChangeAnim("die");
        }
    }
}
