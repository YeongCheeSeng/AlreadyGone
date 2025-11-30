using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] public Transform enemy;

    [Header("Movement")]
    [SerializeField] public float speed;
    private Vector3 initscale;
    private bool movingLeft;

    public bool canMove = true;
    public bool canFollow = false;

    private E_Health enemyHealth;
    public Animator animator;

    void Awake()
    {
        initscale = enemy.localScale;
    }

    private void Start()
    {
        enemyHealth = enemy.gameObject.GetComponent<E_Health>();
    }

    void Update()
    {
        if (canFollow) return; // if can follow is true return

        if (!canMove) return;

        if(movingLeft)
        {
            if(enemy.position.x >= leftEdge.position.x)
            {
                MoveInDirection(-1);    
            }
            else
            {
                DirectionChange();
            }
                
        }
        else
        {
            if(enemy.position.x <= rightEdge.position.x)
            {
                MoveInDirection(1);    
            }
            else
            {
                DirectionChange();
            }
        }
        
    }

    private void DirectionChange()
    {
        movingLeft = !movingLeft;
    }

    public void MoveInDirection(int _direction)
    {
        if (enemyHealth != null)
        {
            if (enemyHealth.dead == true) return;

            //animator.SetTrigger("moving");

            enemy.localScale = new Vector3(Mathf.Abs(initscale.x) * _direction,
                initscale.y, initscale.z);

            enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed,
                enemy.position.y, enemy.position.z);
        }
    }
}
