using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth;
    private Animator anim;
    private bool dead;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        
        if (currentHealth > 0)
        {
            //enemy hurt
            anim.SetTrigger("hurt");  
        }
        else
        {
            // enemy dead
           if(!dead)
           {
                anim.SetTrigger("die");
                dead = true;
           }
        }
    }
    
    private void Update() // TEST PURPOSE
    {
        if(Input.GetKeyDown(KeyCode.E))
            TakeDamage(1);   
    }
}
