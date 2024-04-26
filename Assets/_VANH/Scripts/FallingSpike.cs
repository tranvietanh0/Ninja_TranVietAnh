using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpike : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float distance = 7f;
    private BoxCollider2D boxCollider2D;
    private bool isFalling = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        CheckFalling();
    }

    private void CheckFalling()
    {
        Physics2D.queriesStartInColliders = false;
        if (isFalling == false)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distance);
            if (hit.transform != null)
            {
                if (hit.transform.tag == "Player")
                {
                    rb.gravityScale = 4;
                    isFalling = true;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            StartCoroutine(DestroySpike());
        }

    }
    
    IEnumerator DestroySpike()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
