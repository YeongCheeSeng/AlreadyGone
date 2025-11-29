using System;
using System.Collections;
using UnityEngine;

public class CinematicBar : MonoBehaviour
{
    public GameObject topBar;
    public GameObject bottomBar;
    public float barSpeed = 2.0f;
    public float TopBarTargetY = -3.5f;
    public float BottomBarTargetY = 3.5f;

    // Initial positions
    private float initialTopBarY;
    private float initialBottomBarY;
    private bool isAnimating = false;

    // Events for animation completion
    public event Action OnBarsShown;
    public event Action OnBarsHidden;

    void Start()
    {
        if (topBar == null || bottomBar == null)
        {
            //Debug.LogError("CinematicBar: Top or Bottom bar is not assigned!", gameObject);
            return;
        }

        initialBottomBarY = bottomBar.transform.position.y;
        initialTopBarY = topBar.transform.position.y;
    }

    private void Update()
    {
        // Optional: You can add debug keys to show/hide bars for testing
        if (Input.GetKeyDown(KeyCode.O))
        {
            HideBars();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            ShowBars();
        }
    }

    public void ShowBars()
    {
        if (isAnimating)
            StopAllCoroutines();
        
        StartCoroutine(MoveBars(initialTopBarY, initialBottomBarY, OnBarsShown));
    }

    public void HideBars()
    {
        if (isAnimating)
            StopAllCoroutines();
        
        StartCoroutine(MoveBars(TopBarTargetY, BottomBarTargetY, OnBarsHidden));
    }

    public bool IsAnimating => isAnimating;

    private IEnumerator MoveBars(float targetTopY, float targetBottomY, Action onComplete = null)
    {
        isAnimating = true;
        float elapsedTime = 0f;
        float totalDistance = Mathf.Abs(topBar.transform.position.y - targetTopY);
        Vector3 topStartPosition = topBar.transform.position;
        Vector3 bottomStartPosition = bottomBar.transform.position;

        // Prevent division by zero
        if (totalDistance < 0.01f)
        {
            isAnimating = false;
            onComplete?.Invoke();
            yield break;
        }

        float duration = totalDistance / barSpeed;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // Move top bar
            Vector3 topPosition = topBar.transform.position;
            topPosition.y = Mathf.Lerp(topStartPosition.y, targetTopY, t);
            topBar.transform.position = topPosition;

            // Move bottom bar
            Vector3 bottomPosition = bottomBar.transform.position;
            bottomPosition.y = Mathf.Lerp(bottomStartPosition.y, targetBottomY, t);
            bottomBar.transform.position = bottomPosition;

            yield return null;
        }

        // Ensure final positions are exact
        topBar.transform.position = new Vector3(topBar.transform.position.x, targetTopY, topBar.transform.position.z);
        bottomBar.transform.position = new Vector3(bottomBar.transform.position.x, targetBottomY, bottomBar.transform.position.z);

        isAnimating = false;
        onComplete?.Invoke();
    }
}
