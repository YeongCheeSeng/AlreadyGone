using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public Image healthBar;

    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null) healthBar.fillAmount = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthBar();

        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10);
            Debug.Log("Player Health: " + currentHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    private void Die()
    {
        Debug.Log("Player Died");
    }
}
