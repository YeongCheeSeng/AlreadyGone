using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    [Header("Flash randomizer")]
    public float maxTime = 30f;
    public float minTime = 20f;
    private float currentTime;

    [Header("Flash setting")]
    public Image flashImage;
    public float flashDuration = 0.1f;
    public int flashCount = 3;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (flashImage != null)
        {
            flashImage.enabled = false;
        }

        currentTime = Random.Range(minTime, maxTime);
    }

    private void Update()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            StartCoroutine(Flash());
            currentTime = Random.Range(minTime, maxTime);
        }
    }

    IEnumerator Flash()
    {
        if (flashImage == null)
        {
            yield break;
        }

        audioSource.Play();

        for (int i = 0; i < flashCount; i++)
        {
            yield return StartCoroutine(FlashOnce());
            yield return new WaitForSeconds(flashDuration);
        }
    }

    IEnumerator FlashOnce()
    {
        flashImage.enabled = true;
        yield return new WaitForSeconds(0.1f);
        flashImage.enabled = false;
    }
}
