using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public GameObject player;
    private PlayerMovement playerMovement;

    [SerializeField] private Renderer bgRenderer;
    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        playerMovement = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement != null)
        {
            bgRenderer.material.mainTextureOffset += new Vector2(playerMovement.speed * Time.deltaTime, 0);
        }
    }
}
