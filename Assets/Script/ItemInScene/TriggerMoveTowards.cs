using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMoveTowards : MonoBehaviour
{
    //BoxCollider2D bc2d;
    Rigidbody2D rb2d;
    private MoveToward moveToward;

    // Start is called before the first frame update
    void Start()
    {
        //bc2d = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        moveToward = GetComponent<MoveToward>();

        if(rb2d != null)
        rb2d.bodyType = RigidbodyType2D.Static;

        if (moveToward != null)
            moveToward.enabled = false;
        else if (moveToward == null)
            Debug.LogWarning(gameObject.name + " has no MoveToward script");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(Wait(0.2f));
        }
    }

    IEnumerator Wait(float second)
    { 
        yield return new WaitForSeconds(second);

        //rb2d.bodyType = RigidbodyType2D.Dynamic;

        if (moveToward != null)
            moveToward.enabled = true;
    }
}
