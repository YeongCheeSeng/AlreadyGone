using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ShowVisualOnce : MonoBehaviour
{
    public Sprite[] sprites;

    public float MinOffset = 0.9f;
    public float MaxOffset = 1.1f;

    public float MinScale = 0.9f;
    public float MaxScale = 1.1f;

    private SpriteRenderer spriteRender;

    // Start is called before the first frame update
    void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();

        if (sprites != null && sprites.Length > 0)
        {
            //spriteRender.transform.position = Random.Range(MinOffset, MaxOffset);
            //spriteRender.volume = Random.Range(MinVolume, MaxVolume);
            //int randomAudioClip = Random.Range(0, AudioClips.Length);

            //sprites.PlayOneShot(AudioClips[randomAudioClip]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (spriteRender != null && spriteRender)
            return;

        Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void SpawnFeedback(GameObject[] Feedbacks)
    {
        foreach (var feedback in Feedbacks)
        {
            GameObject FeedbackClone = GameObject.Instantiate(feedback, transform.position, transform.rotation);
            Destroy(FeedbackClone, 1f);
        }
    }
}
