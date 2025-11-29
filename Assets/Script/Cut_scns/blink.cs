using UnityEngine;
using TMPro;

public class BlinkDelayed : MonoBehaviour
{
    public TextMeshProUGUI text;    // Assign your Continue Text
    public float delay = 2f;        // Time in seconds before text appears
    public float speed = 5f;        // Blink speed

    private bool startBlinking = false;

    void Start()
    {
        text.alpha = 0;              // Start invisible
        Invoke(nameof(StartBlinking), delay); // Call StartBlinking after delay
    }
    // Somewhere here add a function to  check if E or continue button has been pressed then set bool scenePlayed to true.

    void StartBlinking()
    {
        startBlinking = true;
    }

    void Update()
    {
        if (startBlinking)
        {
            float alpha = Mathf.Abs(Mathf.Sin(Time.time * speed));
            text.alpha = alpha;
        }
    }
}
