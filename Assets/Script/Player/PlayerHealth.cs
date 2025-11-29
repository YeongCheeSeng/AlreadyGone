using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public Image healthBar;
    public bool isDead = false;
    public GameObject[] dieFeedback;

    public float currentHealth;
    private PlayerMovement playerMovement;
    private BoxCollider2D boxColider;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        boxColider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;

        if (healthBar != null) healthBar.fillAmount = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void RecoverHealth(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
    }

    private void Die()
    {
        Debug.Log("Player Died");
        rb.gravityScale = 0;
        boxColider.enabled = false;
        playerMovement.SetCanMove(false);
        FeedbackManager.Instance.SpawnFeedback(dieFeedback);
        isDead = true;
    }
}
