using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoint : MonoBehaviour
{
    public CheckpointManager checkpointManager;
    private BoxCollider2D bc2d;

    // Start is called before the first frame update
    void Start()
    {
        checkpointManager = GetComponentInParent<CheckpointManager>();
        bc2d = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            bc2d.enabled = false;
            checkpointManager.CheckPlayerCurrentCheckpoint();
        }
    }
}
