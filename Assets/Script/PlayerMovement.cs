using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public Animator animator;

    private Rigidbody2D rb;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleMovement();
        HandleAnimations();

        Debug.Log("Is Grounded: " + isGrounded);
    }

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, speed);
        }
    }

    private void HandleAnimations()
    {

        if (rb.velocity.x != 0 && isGrounded)
        {
            animator.Play("Player_Walk");
        }
        else if (isGrounded)
        {
            animator.Play("Player_Idle");
        }
        else
        {
            animator.Play("Player_Jump");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
