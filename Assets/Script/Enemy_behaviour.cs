using System.Collections;
using UnityEngine;

public class Enemy_behaviour : MonoBehaviour
{
    [SerializeField] private float attackcooldown;
    [SerializeField] private float range;
    [SerializeField] private float collisionDistance;
    [SerializeField] private int damage;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float velocityThreshold = 0.05f;
    [SerializeField] private float attackAnimationDelay = 0.3f;
    
    [Header("Jump")]
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float jumpDistance = 3f;
    [SerializeField] private float jumpAwayRange = 2f;
    [SerializeField] private float groundVelocityThreshold = 0.1f;
    [SerializeField] private float jumpDuration = 0.8f;
    [SerializeField] private float jumpCooldown = 1.5f;
    
    public GameObject[] hitFeedback;
    private float cooldowntimer = Mathf.Infinity;
    private float jumpCooldownTimer = 0f;
    private float initSpeed;

    public Animator anim;
    public GameObject spawnPos;
    private PlayerHealth player_health;
    private E_Health enemy_health;
    private EnemyPatrol enemy_patrol;
    private Rigidbody2D rb;
    private bool isBeingControlledByFollow = false;
    private bool isAttacking = false;
    private bool isJumping = false;
    private bool isGrounded = true;
    private Transform playerTransform;
    private Coroutine jumpCoroutine;

    private void Awake()
    {
        enemy_patrol = GetComponentInParent<EnemyPatrol>();
        enemy_health = GetComponent<E_Health>();
        rb = GetComponent<Rigidbody2D>();

        initSpeed = enemy_patrol.speed;
    }

    private void Update() 
    {
        if (enemy_health.dead == true)
        {
            anim.SetBool("isAttacking", false);
            
            // Stop jump coroutine if enemy dies
            if (isJumping && jumpCoroutine != null)
            {
                StopCoroutine(jumpCoroutine);
                isJumping = false;
            }
            
            return;
        }

        // Decrement jump cooldown
        jumpCooldownTimer -= Time.deltaTime;

        // Check if grounded
        isGrounded = Mathf.Abs(rb.velocity.y) <= groundVelocityThreshold;

        cooldowntimer += Time.deltaTime;

        // Only handle animation if not being controlled by EnemyFollow and not attacking
        if (!isBeingControlledByFollow && !isAttacking && !isJumping)
        {
            PlayRunningAnimation();
        }

        // Attack only when player insight
        if(playerInsight() && cooldowntimer > attackcooldown && !isAttacking && !isJumping)
        {
            StartCoroutine(Attack());
            enemy_patrol.canMove = false;
        }

        if(!playerInsight())
        {
            enemy_patrol.canMove = true;
        }    
    }

    public void PlayRunningAnimation()
    {
        anim.SetBool("isAttacking", false);
        
        if (Mathf.Abs(rb.velocity.x) > velocityThreshold)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
    }

    public void StopRunningAnimation()
    {
        anim.SetBool("isRunning", false);
        anim.SetBool("isAttacking", false);
    }

    public void SetFollowControl(bool isControlled)
    {
        isBeingControlledByFollow = isControlled;
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    public bool IsJumping()
    {
        return isJumping;
    }

    public void TriggerJumpAway()
    {
        if (!isJumping && isGrounded && jumpCooldownTimer <= 0)
        {
            jumpCoroutine = StartCoroutine(JumpAway());
            jumpCooldownTimer = jumpCooldown;
        }
    }

    private bool playerInsight()
    {
        RaycastHit2D hit = 
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * collisionDistance, 
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        {
            playerTransform = hit.transform;
        }

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * collisionDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z ));
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        cooldowntimer = 0;
        
        // Stop movement during attack
        rb.velocity = new Vector2(0, rb.velocity.y);
        
        // Set animator parameters to trigger attack
        anim.SetBool("isRunning", false);
        anim.SetBool("isAttacking", true);
        
        // Wait for the attack animation to reach the damage point
        yield return new WaitForSeconds(attackAnimationDelay);
        
        // Apply damage at the synchronized point
        ApplyAttackDamage();
        
        // Wait for the rest of the attack animation to finish
        yield return new WaitForSeconds(0.7f - attackAnimationDelay);
        
        anim.SetBool("isAttacking", false);
        isAttacking = false;
        
        // Jump away after hitting the player
        if (playerInsight())
        {
            yield return new WaitForSeconds(0.2f);
            TriggerJumpAway();
        }
    }

    private void ApplyAttackDamage()
    {
        RaycastHit2D hit = 
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * collisionDistance, 
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        {
            player_health = hit.transform.GetComponent<PlayerHealth>();
            player_health?.TakeDamage(damage);

            // Show feedback
            FeedbackManager.Instance.SpawnFeedback(hitFeedback, spawnPos);
        }
    }

    IEnumerator JumpAway()
    {
        isJumping = true;
        
        // Determine jump direction (away from player)
        float directionAwayFromPlayer = 1f;
        if (playerTransform != null)
        {
            // Calculate direction away from player
            // If player is to the right, jump left (negative)
            // If player is to the left, jump right (positive)
            float directionToPlayer = Mathf.Sign(playerTransform.position.x - transform.position.x);
            directionAwayFromPlayer = -directionToPlayer;
        }
        else
        {
            // Fallback: jump opposite to facing direction
            directionAwayFromPlayer = -transform.localScale.x;
        }
        
        // Apply jump velocity
        rb.velocity = new Vector2(directionAwayFromPlayer * jumpDistance, jumpHeight);
        
        // Play jump animation
        anim.SetBool("isRunning", false);
        anim.SetBool("isAttacking", false);
        anim.SetTrigger("jump");
        
        // Wait for jump to complete (don't let other scripts interfere)
        float jumpTimer = 0f;
        while (jumpTimer < jumpDuration && !enemy_health.dead)
        {
            // Maintain horizontal velocity during jump
            rb.velocity = new Vector2(directionAwayFromPlayer * jumpDistance, rb.velocity.y);
            jumpTimer += Time.deltaTime;
            yield return null;
        }
        
        isJumping = false;
    }

    void damagePlayer()
    {
        if(playerInsight())
        {
            player_health.TakeDamage(damage);
        }
    }
}

