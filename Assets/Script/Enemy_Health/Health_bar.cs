using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

// Neeed to add UI CANVAS FOR HEALTH BAR

public class Health_bar : MonoBehaviour
{
    [SerializeField] private E_Health enemyHealth;
    [SerializeField] private Image totalHealthbar;
    [SerializeField] private Image currentHealthbar;

    private void Start() 
    {
        
    }
    private void Update() 
    {
        currentHealthbar.fillAmount = enemyHealth.currentHealth / 10;    
    }
}
