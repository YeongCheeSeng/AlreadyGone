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

    public void CheckPlayerCurrentCheckpoint() //this is trigger by checkpoint.cs
    {
        index++;
        index = Mathf.Clamp(index, 0, checkpoints.Count - 1); // <--- this code means the number of index will not lower than 0 and higher than checkpoints.Count - 1

        currentCheckpoint = checkpoints[index];
    }

    void RespawnPlayer() 
    {
        if (playerHealth != null && playerHealth.currentHealth == 0)
        { 
            playerHealth.currentHealth = playerHealth.maxHealth;
            player.transform.position = currentCheckpoint.transform.position;
        }
    }
}
