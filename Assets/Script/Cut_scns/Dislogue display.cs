using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class DialogueSequence : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public TextMeshProUGUI dialogueText;     // The main dialogue text
    public string dialogueLine;              // The line to print out
    public float typingSpeed = 0.04f;        // Speed of typing
    public float waitAfterFinished = 2f;     // Wait time before showing continue text

    [Header("Continue Prompt")]
    public TextMeshProUGUI continueText;     // "Press E to continue"
    public float blinkSpeed = 5f;            // Blink speed for continue text

    [Header("Scene")]
    public string nextSceneName = "Level2";  // Scene to load on E press

    private bool canPressE = false;

    void Start()
    {
        continueText.alpha = 0;   // Hide continue prompt
        dialogueText.text = "";
        StartCoroutine(TypeDialogue());
    }

    void Update()
    {
        if (canPressE)
        {
            // Blink effect
            continueText.alpha = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));

            if (Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }

    IEnumerator TypeDialogue()
    {
        // Type out characters one by one
        foreach (char c in dialogueLine)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Wait before showing continue prompt
        yield return new WaitForSeconds(waitAfterFinished);

        canPressE = true;
    }
}
