using UnityEngine.UI;
using UnityEngine;

public class E_Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth;
    public Animator anim;
    public Image bar;
    public bool dead;
    public GameObject[] dieFeedback;

    public BoxCollider2D boxCollider;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = startingHealth;       
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
                boxCollider.enabled = false;
                rb.velocity = Vector3.zero;
                rb.gravityScale = 0f;
                FeedbackManager.Instance.SpawnFeedback(dieFeedback, gameObject);
           }
        }
    }
    
    private void Update() // TEST PURPOSE
    {
        //if(Input.GetKeyDown(KeyCode.E))
        //    TakeDamage(1);   

        bar.fillAmount = currentHealth / startingHealth;
    }
}
