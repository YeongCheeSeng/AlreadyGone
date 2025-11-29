using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPointZone : MonoBehaviour
{
    [SerializeField] private float moveDuration = 2.8f; // Duration for moving the player
    [SerializeField] private float waitBeforeLoad = 5f; // Wait time before loading next scene
    public bool walkingInStart = true;
    Collider2D endGameZone;
    private PlayerMovement player;
    private Rigidbody2D playerRigidbody;

    private void Awake()
    {
        endGameZone = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Player")
        {
            Debug.Log("Player reached the end zone.");
            player = target.GetComponent<PlayerMovement>();

            if (player != null)
            {
                // Cache the Rigidbody2D component
                playerRigidbody = target.GetComponent<Rigidbody2D>();
                
                // Disable player controls
                player.SetCanMove(false);
                StartCoroutine(AutoMove());
                endGameZone.enabled = false; // Disable the collider to prevent re-triggering
            }
        }
    }

    private IEnumerator AutoMove()
    {
        float elapsedTime = 0f;
        Vector2 originalVelocity = playerRigidbody.velocity;

        // Play walk animation once
        player.animator.Play("Player_Walk");

        // Move player to the right for moveDuration seconds
        while (elapsedTime < moveDuration)
        {
            playerRigidbody.velocity = new Vector2(player.speed, originalVelocity.y);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Stop movement
        playerRigidbody.velocity = new Vector2(0, originalVelocity.y);
        player.animator.Play("Player_Idle");

        if (walkingInStart)
        {
            player.SetCanMove(true);
        }
        else
        {
            // Wait for seconds before loading next scene
            yield return new WaitForSeconds(waitBeforeLoad);

            // Load the next scene
            //SceneManager.LoadScene("Ending");
            Debug.Log("Loading Ending Scene...");
        }
    }
}
