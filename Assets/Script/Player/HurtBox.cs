using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    public GameObject[] hitFeedback;
    private bool damaged = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && damaged == false)
        {
            E_Health enemyHealth = collision.GetComponent<E_Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(1);
                FeedbackManager.Instance.SpawnFeedback(hitFeedback,gameObject);
            }

            damaged = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            damaged = false;
        }
    }
}
