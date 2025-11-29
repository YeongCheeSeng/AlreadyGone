using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Enemy_behaviour : MonoBehaviour
{
    [SerializeField] private float attackcooldown;
    [SerializeField] private float range;
    [SerializeField] private float collisionDistance;
    [SerializeField] private int damage;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    public GameObject[] hitFeedback;
    private float cooldowntimer = Mathf.Infinity;
    private float initSpeed;

    public Animator anim;
    private PlayerHealth player_health;
    private E_Health enemy_health;
    private EnemyPatrol enemy_patrol;

    private void Awake()
    {
        enemy_patrol = GetComponentInParent<EnemyPatrol>();
        enemy_health = GetComponent<E_Health>();

        initSpeed = enemy_patrol.speed;
    }

    private void Update() 
    {
        if (enemy_health.dead == true) return;

        cooldowntimer += Time.deltaTime;

        // Attack only when player insight
        if(playerInsight())
        {
            if(cooldowntimer > attackcooldown)
            {
                //cooldowntimer = 0;
                //anim.SetTrigger("attack"); 

                StartCoroutine(Attack());
            }       
            enemy_patrol.canMove = false;
        }

        if(!playerInsight())
        {
            enemy_patrol.canMove = true;
        }    
    }

    private bool playerInsight()
    {
        RaycastHit2D hit = 
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * collisionDistance, 
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null && cooldowntimer > attackcooldown)
        {
            player_health = hit.transform.GetComponent<PlayerHealth>();
            player_health?.TakeDamage(damage);
            FeedbackManager.Instance.SpawnFeedback(hitFeedback);
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
        cooldowntimer = 0;
        anim.SetTrigger("attack");
        yield return new WaitForSeconds(3f);
    }

    // private bool playerInsight()
    // {
    //     // Determine cast direction based on enemy facing
    //     Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    //     // Cast box forward
    //     RaycastHit2D hit = Physics2D.BoxCast(
    //         boxCollider.bounds.center,
    //         new Vector2(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y),
    //         0f,
    //         direction,
    //         collisionDistance,
    //         playerLayer
    //     );

    //     return hit.collider != null;
    // }

    // private void OnDrawGizmos()
    // {
    //     if (boxCollider == null) return;

    //     Gizmos.color = Color.red;
    //     Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    //     Gizmos.DrawWireCube(
    //         boxCollider.bounds.center + (Vector3)(direction * collisionDistance),
    //         new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, 1)
    //     );
    // }

    void damagePlayer()
    {
        if(playerInsight())
        {
            player_health.TakeDamage(damage);
        }
    }

}

