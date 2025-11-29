using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ShowVisualOnce : MonoBehaviour
{
    public Sprite[] sprites;

    public float ShowTime = 0.1f;

    private SpriteRenderer spriteRender;

    // Start is called before the first frame update
    void Start()
    {
        spriteRender = GetComponentInChildren<SpriteRenderer>();

        if (sprites != null && sprites.Length > 0)
        {
            int randomSprite = Random.Range(0, sprites.Length);
            spriteRender.sprite = sprites[randomSprite];

            Die(ShowTime);
        }
    }

    void Die(float dur)
    {
        Destroy(gameObject, dur);
    }
}
