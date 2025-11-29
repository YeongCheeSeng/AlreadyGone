using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recover : MonoBehaviour
{
    public float RecoverAmount = 20f;
    private GameObject player;
    private bool hasRecovered = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasRecovered) return;

        if (other.gameObject == player)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

            if (playerHealth != null && playerHealth.currentHealth < playerHealth.maxHealth)
            {
                playerHealth.RecoverHealth(RecoverAmount);
                hasRecovered = true;
                Destroy(gameObject);
            }
        }
    }
}
