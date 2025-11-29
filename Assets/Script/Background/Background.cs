using UnityEngine;

public class Background : MonoBehaviour
{
    public GameObject player;
    private Vector3 lastPos;

    public float parallaxMultiplier = 0.05f;
    [SerializeField] private Renderer bgRenderer;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        lastPos = player.transform.position;
    }

    void Update()
    {
        // Follow player horizontally
        transform.position = new Vector3(player.transform.position.x, transform.position.y,transform.position.z);

        float deltaX = player.transform.position.x - lastPos.x;

        // Move background only when actual position changes
        bgRenderer.material.mainTextureOffset += new Vector2(deltaX * parallaxMultiplier, 0);

        lastPos = player.transform.position;
    }
}