using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPointZone : MonoBehaviour
{
    [SerializeField] private float moveDuration = 2.8f; // Duration for moving the player
    [SerializeField] private float waitBeforeLoad = 5f; // Wait time before loading next scene
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
                StartCoroutine(VictorySequence());
                endGameZone.enabled = false; // Disable the collider to prevent re-triggering
            }
        }
    }

    private IEnumerator VictorySequence()
    {
        float elapsedTime = 0f;
        Vector2 originalVelocity = playerRigidbody.velocity;

        // Move player to the right for moveDuration seconds
        while (elapsedTime < moveDuration)
        {
            playerRigidbody.velocity = new Vector2(player.speed, originalVelocity.y);
            //player.HandleAnimations(); // Update animations if necessary
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Wait for seconds before loading next scene
        yield return new WaitForSeconds(waitBeforeLoad);

        // Put loading next scene here
        Debug.Log("Loading Next Scene");
    }
}
