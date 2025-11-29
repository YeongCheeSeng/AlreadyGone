using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    private PlayerMovement playerMovement;
    public GameObject bar;
    
    void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    void LateUpdate()
    {
        if (playerMovement == null) return;

        if (playerMovement.isFacingRight)
            bar.transform.localScale = new Vector3(Mathf.Abs(bar.transform.localScale.x), bar.transform.localScale.y);
        else
            bar.transform.localScale = new Vector3(-Mathf.Abs(bar.transform.localScale.x), bar.transform.localScale.y);
    }
}
