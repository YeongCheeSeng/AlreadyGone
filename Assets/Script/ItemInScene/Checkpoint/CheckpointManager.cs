using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public List<Transform> checkpoints = new List<Transform>();
    public int index = 0;
    public Transform currentCheckpoint;
    protected GameObject player;
    private PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();

        foreach (Transform t in GetComponentsInChildren<Transform>()) 
        {
            if (t.name.ToLower().Contains("checkpoint"))
            {
                if (t == this.transform)
                {
                    continue;
                }

                checkpoints.Add(t);
            }
        }       
    }

    // Update is called once per frame
    void Update()
    {
        RespawnPlayer();
    }

    protected void CheckPlayerCurrentCheckpoint()
    {
        if (Vector2.Distance(checkpoints[index].gameObject.transform.position, player.transform.position) < 1f)
        {
            if (index < checkpoints.Count -1)
            index++;

            checkpoints[index].gameObject.SetActive(false);
            
        }
    }

    void RespawnPlayer() 
    {
        if (playerHealth != null && playerHealth.currentHealth == 0)
        { 
            index = Mathf.Clamp(index, 0, checkpoints.Count - 1);

            playerHealth.currentHealth = playerHealth.maxHealth;
            player.transform.position = checkpoints[index].position;
        }
    }
}
