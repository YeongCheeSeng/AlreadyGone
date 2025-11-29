using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;

    [Header("Movement")]
    [SerializeField] private float speed;
    private Vector3 initscale;
    private bool movingLeft;
    void Awake()
    {
        initscale = enemy.localScale;
    }

    void Update()
    {
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

    private void MoveInDirection(int _direction)
    {
        enemy.localScale = new Vector3(Mathf.Abs(initscale.x) * _direction, 
            initscale.y, initscale.z);

        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed, 
            enemy.position.y, enemy.position.z);
    }
}
