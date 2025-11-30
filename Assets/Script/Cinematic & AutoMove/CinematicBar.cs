using System;
using System.Collections;
using UnityEngine;

public class CinematicBar : MonoBehaviour
{
    public GameObject topBar;
    public GameObject bottomBar;
    public float barSpeed = 2.0f;
    public float topBarScreenY = 0.9f;  // Screen space Y (0 = bottom, 1 = top)
    public float bottomBarScreenY = 0.1f;  // Screen space Y (0 = bottom, 1 = top)
    public float offScreenOffset = 0.15f;  // How far off-screen to move bars when hiding

    // Initial positions
    private Vector3 initialTopBarPosition;
    private Vector3 initialBottomBarPosition;
    private bool isAnimating = false;
    private Camera mainCamera;
    private float cachedOrthoSize;
    private Vector3 cachedCameraPos;

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

        mainCamera = GetComponent<Camera>();
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // Cache initial camera state
        CacheCameraState();

        initialTopBarPosition = topBar.transform.position;
        initialBottomBarPosition = bottomBar.transform.position;
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

    private void CacheCameraState()
    {
        cachedOrthoSize = mainCamera.orthographicSize;
        cachedCameraPos = mainCamera.transform.position;
    }

    public void ShowBars()
    {
        if (isAnimating)
            StopAllCoroutines();
        
        CacheCameraState();
        StartCoroutine(AnimateBars(topBarScreenY, bottomBarScreenY, OnBarsShown));
    }

    public void HideBars()
    {
        if (isAnimating)
            StopAllCoroutines();
        
        CacheCameraState();
        StartCoroutine(AnimateBars(1f + offScreenOffset, -offScreenOffset, OnBarsHidden));
    }

    public bool IsAnimating => isAnimating;

    private Vector3 GetScreenSpaceWorldPosition(float screenX, float screenY)
    {
        // Use cached ortho size instead of current camera state
        Vector3 screenPos = new Vector3(Screen.width * screenX, Screen.height * screenY, 10f);
        
        // Calculate world position using cached ortho size
        float worldHeight = cachedOrthoSize * 2f;
        float worldWidth = worldHeight * (Screen.width / (float)Screen.height);
        
        float worldX = cachedCameraPos.x - (worldWidth / 2f) + (worldWidth * screenX);
        float worldY = cachedCameraPos.y - (worldHeight / 2f) + (worldHeight * screenY);
        
        return new Vector3(worldX, worldY, 10f);
    }

    private IEnumerator AnimateBars(float targetTopScreenY, float targetBottomScreenY, Action onComplete = null)
    {
        isAnimating = true;
        float elapsedTime = 0f;
        
        // Get initial screen Y positions using cached camera state
        Vector3 topWorldPos = topBar.transform.position;
        Vector3 bottomWorldPos = bottomBar.transform.position;
        
        float worldHeight = cachedOrthoSize * 2f;
        float startTopScreenY = ((topWorldPos.y - cachedCameraPos.y) + (worldHeight / 2f)) / worldHeight;
        float startBottomScreenY = ((bottomWorldPos.y - cachedCameraPos.y) + (worldHeight / 2f)) / worldHeight;
        
        // Calculate duration based on screen distance
        float screenDistance = Mathf.Abs(targetTopScreenY - startTopScreenY);
        float duration = screenDistance > 0.01f ? screenDistance / barSpeed : 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // Interpolate screen space Y positions
            float currentTopScreenY = Mathf.Lerp(startTopScreenY, targetTopScreenY, t);
            float currentBottomScreenY = Mathf.Lerp(startBottomScreenY, targetBottomScreenY, t);

            // Convert screen space to world space and update positions
            topBar.transform.position = GetScreenSpaceWorldPosition(0.5f, currentTopScreenY);
            bottomBar.transform.position = GetScreenSpaceWorldPosition(0.5f, currentBottomScreenY);

            yield return null;
        }

        // Ensure final positions are exact
        topBar.transform.position = GetScreenSpaceWorldPosition(0.5f, targetTopScreenY);
        bottomBar.transform.position = GetScreenSpaceWorldPosition(0.5f, targetBottomScreenY);

        isAnimating = false;
        onComplete?.Invoke();
    }
}
