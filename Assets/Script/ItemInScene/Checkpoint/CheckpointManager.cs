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
    private PlayerMovement playerMovement;
    public float waitBeforeRespawn = 1f;
    private bool once;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        playerMovement = player.GetComponent<PlayerMovement>();

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

        if (checkpoints.Count > 0)
        {
            currentCheckpoint = checkpoints[0];
            currentCheckpoint.transform.position = new Vector2(player.transform.position.x, currentCheckpoint.transform.position.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth != null && playerHealth.currentHealth == 0 && once == false)
        {
            StartCoroutine(Wait(waitBeforeRespawn));
        }
    }

    public void CheckPlayerCurrentCheckpoint() //this is trigger by checkpoint.cs
    {
        index++;
        index = Mathf.Clamp(index, 0, checkpoints.Count - 1); // <--- this code means the number of index will not lower than 0 and higher than checkpoints.Count - 1

        if (checkpoints.Count > 0)
        currentCheckpoint = checkpoints[index -1];
    }
    IEnumerator Wait(float second)
    {
        once = true;

        yield return new WaitForSeconds(second);
        RespawnPlayer();

        once = false;
    }

    void RespawnPlayer() 
    {
        playerHealth.currentHealth = playerHealth.maxHealth;
        player.transform.position = currentCheckpoint.transform.position;
        playerHealth.isDead = false;
        playerMovement.SetCanMove(true);
        playerMovement.isGrounded = true;
    }

}
