using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    public Image flashImage;
    public float flashDuration = 0.1f;
    public int flashCount = 3;

    void Start()
    {
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        if (flashImage == null)
        {
            yield break;
        }
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
