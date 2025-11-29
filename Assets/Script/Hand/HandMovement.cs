using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMovement : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float speed = 1.0f;

    private Rigidbody2D rb;
    private Vector3 originalTransform;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (target == null)
        {
            target = GameObject.FindWithTag("Player");
        }

        originalTransform = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null || rb == null) return;

        //Movement Towards Target
        rb.velocity = (target.transform.position - transform.position).normalized * speed;

        //Sprite Flipping
        if (rb.velocity.x > 0.1f)
        {
            transform.localScale = new Vector3(-originalTransform.x, originalTransform.y, originalTransform.z);
        }
        else if (rb.velocity.x < -0.1f)
        {
            transform.localScale = new Vector3(originalTransform.x, originalTransform.y, originalTransform.z);
        }

        //Animation Handling
        if (animator != null)
        {
            if (rb.velocity.magnitude > 0.1f)
            {
                animator.Play("Hand_Walk");
            }
            else
            {
                animator.Play("Hand_Idle");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            rb.velocity = Vector3.zero;
        }
    }
}
