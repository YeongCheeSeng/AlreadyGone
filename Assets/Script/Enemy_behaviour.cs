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
    public GameObject[] hitFeedback;
    private float cooldowntimer = Mathf.Infinity;
    private float initSpeed;

    public Animator anim;
    public GameObject spawnPos;
    private PlayerHealth player_health;
    private E_Health enemy_health;
    private EnemyPatrol enemy_patrol;
    private Rigidbody2D rb;
    private bool isBeingControlledByFollow = false;
    private bool isAttacking = false;

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
            StopRunningAnimation();
            return;
        }

        cooldowntimer += Time.deltaTime;

        // Only handle animation if not being controlled by EnemyFollow
        if (!isBeingControlledByFollow)
        {
            PlayRunningAnimation();
        }

        // Attack only when player insight
        if(playerInsight() && cooldowntimer > attackcooldown && !isAttacking)
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
        if (Mathf.Abs(rb.velocity.x) > velocityThreshold)
        {
            anim.Play("running");
        }
        else
        {
            StopRunningAnimation();
        }
    }

    public void StopRunningAnimation()
    {
        anim.Play("idle");
    }

    public void SetFollowControl(bool isControlled)
    {
        isBeingControlledByFollow = isControlled;
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    private bool playerInsight()
    {
        RaycastHit2D hit = 
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * collisionDistance, 
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

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
        
        anim.SetTrigger("attack");
        
        // Wait for the attack animation to reach the damage point
        yield return new WaitForSeconds(attackAnimationDelay);
        
        // Apply damage at the synchronized point
        ApplyAttackDamage();
        
        
        
        // Wait for the rest of the attack animation to finish
        yield return new WaitForSeconds(0.7f - attackAnimationDelay);
        
        isAttacking = false;
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

    void damagePlayer()
    {
        if(playerInsight())
        {
            player_health.TakeDamage(damage);
        }
    }
}

