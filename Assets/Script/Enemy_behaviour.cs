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
    private float cooldowntimer = Mathf.Infinity;
    private float initSpeed;

    private Animator anim;
    private PlayerHealth player_health;
    private EnemyPatrol enemy_patrol;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemy_patrol = GetComponentInParent<EnemyPatrol>();
        initSpeed = enemy_patrol.speed;
    }

    private void Update() 
    {
        cooldowntimer += Time.deltaTime;

        // Attack only when player insight
        if(playerInsight())
        {
            if(cooldowntimer > attackcooldown)
            {
                cooldowntimer = 0;
                anim.SetTrigger("attack"); 
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

        if(hit.collider != null)
            player_health = hit.transform.GetComponent<PlayerHealth>();


        return hit.collider != null;
    }

    private  void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * collisionDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z ));
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

