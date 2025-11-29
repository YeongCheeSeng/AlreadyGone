using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public float jumpHeight = 5f;
    public bool canMove = true;
    public Animator animator;

    [Header("Attack")]
    public GameObject hurtBox;
    public float attackStart = 0.1f;
    public float attackDur = 0.3f;
    public float attackEnd = 0.1f;
    private bool isAttacking;

    [Header("Reference")]
    public bool isFacingRight = true;

    private Rigidbody2D rb;
    private bool isGrounded;
    private PlayerHealth playerHealth;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<PlayerHealth>();

        if (hurtBox != null) hurtBox.SetActive(false);
    }

    private void Update()
    {
        if (canMove)
        {
            HandleMovement();
            HandleJump();
            HandleAttack();
        }

        HandleAnimations();
    }

    private void HandleMovement()
    {
        if (isAttacking) return;

        if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector3(-1, 1, 1);
            isFacingRight = false;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector3(1, 1, 1);
            isFacingRight = true;
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    private void HandleJump()
    {
        if (isAttacking) return;

        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }
    }

    private void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isAttacking)
        {
            Debug.Log("Attack");
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        if (hurtBox == null) yield break;

        isAttacking = true;

        //Start up attack
        rb.velocity = new Vector2(0, rb.velocity.y);
        yield return new WaitForSeconds(attackStart);

        //During attack
        hurtBox.SetActive(true);
        yield return new WaitForSeconds(attackDur);

        //End of attack
        hurtBox.SetActive(false);
        yield return new WaitForSeconds(attackEnd);

        isAttacking = false;
    }

    public void HandleAnimations()
    {
        if (playerHealth.isDead)
        {
            animator.Play("Player_Death");
            return;
        }

        if (!isAttacking)
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
            return;
        }
        else if (isGrounded && isAttacking)
        {
            animator.Play("Player_Attack");
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

    public void SetCanMove(bool value)
    {
        canMove = value;
        if (!canMove)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }
}
