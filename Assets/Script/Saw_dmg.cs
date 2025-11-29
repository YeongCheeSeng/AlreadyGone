using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw_dmg : MonoBehaviour
{

    [SerializeField] private float damage;

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.tag == "Enemy")
        {
            collision.GetComponent<E_Health>().TakeDamage(damage);
        }
    }
}
