using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    private GameObject player;
    public GameObject bar;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        bar.transform.localScale = new Vector2(player.transform.localScale.x, bar.transform.localScale.y);
    }
}
