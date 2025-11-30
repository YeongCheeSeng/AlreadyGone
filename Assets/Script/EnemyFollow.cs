// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class EnemyFollow : MonoBehaviour
// {
// public GameObject player;
// private float distance;
// public float speed;

//     // Start is called before the first frame update
//     void Start()
//     {

//     }

//     // Update is called once per frame
//     void Update()
//     {
//         distance = Vector2.Distance(transform.position, player.transform.position);
//         Vector2 direction = player.transform.position - transform.position;

//         transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
//     }
// }


using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    public float chaseRange = 5f;

    public GameObject enemy;
    private Rigidbody2D rb;
    private float enemyFacing;
    private SpriteRenderer sr;
    private Animator anim;
    private EnemyPatrol enemy_patrol;
    private E_Health enemy_health;
    private Enemy_behaviour enemy_behaviour;

    private PlayerHealth playerHealth;
    private Vector3 originalTransform;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = enemy.GetComponent<Animator>();
        enemy_health = GetComponent<E_Health>();
        enemy_patrol = GetComponentInParent<EnemyPatrol>();
        enemy_behaviour = GetComponent<Enemy_behaviour>();

        enemyFacing = enemy_patrol.enemy.transform.localScale.x;
        originalTransform = transform.localScale;
    }

    private void Update()
    {
        if (enemy_health.dead == true)
        {
            enemy_behaviour.SetFollowControl(false);
            enemy_behaviour.StopRunningAnimation();
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < chaseRange)
        {
            playerHealth = player.GetComponent<PlayerHealth>();

            if (playerHealth.currentHealth > 0)
            {
                enemy_patrol.canFollow = true;
                enemy_behaviour.SetFollowControl(true);
                
                // Don't chase if the enemy is attacking
                if (!enemy_behaviour.IsAttacking())
                {
                    ChasePlayer();
                }
                else
                {
                    // Stop movement during attack
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    enemy_behaviour.StopRunningAnimation();
                }
            }
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            enemy_patrol.canFollow = false;
            enemy_behaviour.SetFollowControl(false);
            enemy_behaviour.StopRunningAnimation();
        }
    }

    private void ChasePlayer()
    {
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);

        if (direction > 0)
            enemy.transform.localScale = new Vector3(originalTransform.x, originalTransform.y, originalTransform.z);
        else
            enemy.transform.localScale = new Vector3(-originalTransform.x, originalTransform.y, originalTransform.z);

        enemy_behaviour.PlayRunningAnimation();
    }
}
