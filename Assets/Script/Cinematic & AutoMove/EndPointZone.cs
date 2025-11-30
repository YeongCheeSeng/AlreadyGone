using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPointZone : MonoBehaviour
{
    [Header("Auto Move Settings")]
    [SerializeField] private float moveDuration = 2.8f; // Duration for moving the player
    [SerializeField] private float waitBeforeLoad = 5f; // Wait time before loading next scene
    [Tooltip("If true, player can walk after auto-move; if false, scene changes after wait time.")]
    public bool walkingAvailable = true; 
    public string nextSceneName;

    [Header("Cinematic Settings")]
    [SerializeField] private GameObject cinematicBars;
    public float cinematicDuration = 5.0f;
    public bool showCinematicBars = true;

    [Header("Spawning Settings")]
    public GameObject spawnPos;
    public GameObject spawnTarget;

    private Collider2D endGameZone;
    private PlayerMovement player;
    private Rigidbody2D playerRigidbody;
    private CinematicBar barSystem;

    private void Awake()
    {
        endGameZone = GetComponent<Collider2D>();

        if (cinematicBars != null)
        {
            barSystem = cinematicBars.GetComponent<CinematicBar>(); // Assign to class-level variable
        }
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Player")
        {
            //Debug.Log("Player reached the end zone.");
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

        // Show bars (starts animating but doesn't wait)
        if (showCinematicBars && barSystem != null)
        {
            barSystem.ShowBars();
        }

        // Move player to the right for moveDuration seconds
        while (elapsedTime < moveDuration)
        {
            playerRigidbody.velocity = new Vector2(player.speed, originalVelocity.y);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Stop movement immediately
        playerRigidbody.velocity = new Vector2(0, originalVelocity.y);
        player.animator.Play("Player_Idle");

        if (spawnPos != null && spawnTarget != null)
        {
            GameObject.Instantiate(spawnTarget, spawnPos.transform.position,spawnPos.transform.rotation );
        }

        if (walkingAvailable)
        {
            // Re-enable player movement immediately after moveDuration
            player.SetCanMove(true);
        }

        // Handle cinematic bars independently
        if (showCinematicBars && barSystem != null)
        {
            // Wait for cinematic duration before hiding bars
            yield return new WaitForSeconds(cinematicDuration);
            barSystem.HideBars();
        }
        
        if (!walkingAvailable)
        {
            // Wait for seconds before loading next scene
            yield return new WaitForSeconds(waitBeforeLoad);

            // Load the next scene
            Debug.Log("Loading next: "+nextSceneName);
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
