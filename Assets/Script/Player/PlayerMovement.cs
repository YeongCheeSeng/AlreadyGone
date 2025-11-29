using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public bool canMove = true;
    public GameObject[] walkFeedback;
    [SerializeField] public float MinFeedbackTime = 0.5f;
    [SerializeField] public float MaxFeedbackTime = 1f;
    private float currentFeedbackTime;  

    [Header("Jump")]
    [SerializeField] public float jumpHeight = 5f;

    [Header("Attack")]
    public GameObject hurtBox;
    [SerializeField] public float attackCooldown = 0.8f;
    private float currentAttackCooldown;

    [Header("Attack detail")]
    [SerializeField] public float attackStart = 0.1f;
    [SerializeField] public float attackDur = 0.3f;
    [SerializeField] public float attackEnd = 0.1f;
    public GameObject[] attackFeedback;
    private bool isAttacking;

    [Header("Walk particle")]
    public GameObject particle;

    [Header("Reference")]
    public Animator animator;
    public bool isFacingRight = true;

    private Rigidbody2D rb;
    public bool isGrounded;
    private PlayerHealth playerHealth;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<PlayerHealth>();

        if (hurtBox != null) hurtBox.SetActive(false);

        currentAttackCooldown = attackCooldown;
        currentFeedbackTime = Random.Range(MinFeedbackTime,MaxFeedbackTime);
    }

    private void Update()
    {
        if (canMove)
        {
            HandleMovement();
            HandleJump();
            HandleAttack();
        }

        HandleFeedback();
        HandleAnimations();

        Debug.Log(isGrounded);
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
        currentAttackCooldown -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isAttacking && currentAttackCooldown <= 0)
        {
            StartCoroutine(Attack());
            currentAttackCooldown = attackCooldown;
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
        FeedbackManager.Instance.SpawnFeedback(attackFeedback, gameObject);
        yield return new WaitForSeconds(attackDur);

        //End of attack
        hurtBox.SetActive(false);
        yield return new WaitForSeconds(attackEnd);

        isAttacking = false;
    }

    public void HandleAnimations()
    {
        if (isFacingRight)
        {
            if (playerHealth.isDead)
            {
                animator.Play("Player_DeathRight");
                return;
            }

            if (!isAttacking)
            {
                if (rb.velocity.x != 0 && isGrounded)
                {
                    animator.Play("Player_WalkRight");
                }
                else if (isGrounded)
                {
                    animator.Play("Player_IdleRight");
                }
                else
                {
                    animator.Play("Player_JumpRight");
                }
            }
            else if (isGrounded && isAttacking)
            {
                animator.Play("Player_AttackRight");
            }
        }
        else
        {
            if (playerHealth.isDead)
            {
                animator.Play("Player_DeathLeft");
                return;
            }

            if (!isAttacking)
            {
                if (rb.velocity.x != 0 && isGrounded)
                {
                    animator.Play("Player_WalkLeft");
                }
                else if (isGrounded)
                {
                    animator.Play("Player_IdleLeft");
                }
                else
                {
                    animator.Play("Player_JumpLeft");
                }
            }
            else if (isGrounded && isAttacking)
            {
                animator.Play("Player_AttackLeft");
            }
        }
    }

    void HandleFeedback()
    {
        if (rb.velocity.magnitude > 0.1f && isGrounded)
        {
            currentFeedbackTime -= Time.deltaTime;

            if (currentFeedbackTime <= 0)
            {
                FeedbackManager.Instance.SpawnFeedback(walkFeedback,gameObject);
                currentFeedbackTime = Random.Range(MinFeedbackTime, MaxFeedbackTime);
            }            
        }

        particle.SetActive(isGrounded);
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
            rb.velocity = new Vector2(0, 0);
        }
    }
}
