using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpawnFeedback(GameObject[] Feedbacks)
    {
        foreach (var feedback in Feedbacks)
        {
            GameObject FeedbackClone = GameObject.Instantiate(feedback, transform.position, transform.rotation);
            Destroy(FeedbackClone, 3f);
        }
    }
}
