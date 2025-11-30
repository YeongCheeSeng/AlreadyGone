// using UnityEngine;
// using TMPro;
// using System.Collections;
// using UnityEngine.SceneManagement;

// public class DialogueSequence : MonoBehaviour
// {
//     [Header("Dialogue Settings")]
//     public TextMeshProUGUI dialogueText;     // The main dialogue text
//     // public string dialogueLine;              // The line to print out
//     [TextArea(3, 6)]
//     public string[] dialogueLine;

//     public float typingSpeed = 0.04f;        // Speed of typing
//     public float waitAfterFinished = 2f;     // Wait time before showing continue text

//     [Header("Continue Prompt")]
//     public TextMeshProUGUI continueText;     // "Press E to continue"
//     public float blinkSpeed = 5f;            // Blink speed for continue text

//     [Header("Scene")]
//     public string nextSceneName = "Level2";  // Scene to load on E press

//     private bool canPressE = false;

//     void Start()
//     {
//         continueText.alpha = 0;   // Hide continue prompt
//         dialogueText.text = "";
//         StartCoroutine(TypeDialogue());
//     }

//     void Update()
//     {
//         if (canPressE)
//         {
//             // Blink effect
//             continueText.alpha = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));

//             if (Input.GetKeyDown(KeyCode.E))
//             {
//                 SceneManager.LoadScene(nextSceneName);
//             }
//         }
//     }

//     IEnumerator TypeDialogue()
//     {
//         // Type out characters one by one
//         foreach (char c in dialogueLine)
//         {
//             dialogueText.text += c;
//             yield return new WaitForSeconds(typingSpeed);
//         }

//         // Wait before showing continue prompt
//         yield return new WaitForSeconds(waitAfterFinished);

//         canPressE = true;
//     }
// }

/*
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class DialogueSequence : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public TextMeshProUGUI dialogueText;

    [TextArea(3, 6)]
    public string[] dialogueLines;          // Multiple lines

    public float typingSpeed = 0.04f;
    public float waitAfterFinished = 1f;

    [Header("Continue Prompt")]
    public TextMeshProUGUI continueText;
    public float blinkSpeed = 5f;

    [Header("Scene")]
    public string nextSceneName = "Level2";

    private int index = 0;
    private bool canPressE = false;

    void Start()
    {
        dialogueText.text = "";
        continueText.alpha = 0;
        StartCoroutine(TypeLine());
    }

    void Update()
    {
        if (canPressE)
        {
            continueText.alpha = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));

            if (Input.GetKeyDown(KeyCode.E))
            {
                NextLine();
            }
        }
    }

    IEnumerator TypeLine()
    {
        foreach (char c in dialogueLines[index])
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(waitAfterFinished);
        canPressE = true;
    }

    void NextLine()
    {
        continueText.alpha = 0;
        canPressE = false;

        if (index < dialogueLines.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(TypeLine());
        }
        else
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
*/

using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class DialogueSequence : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public TextMeshProUGUI dialogueText;

    [TextArea(3, 6)]
    public string[] dialogueLines;

    public float typingSpeed = 0.04f;
    public float waitAfterFinished = 0.5f;

    [Header("Continue Prompt")]
    public TextMeshProUGUI continueText;
    public float blinkSpeed = 5f;

    [Header("Scene")]
    public string nextSceneName = "Level2";

    private int index = 0;
    private bool canPressE = false;
    private bool isTyping = false;

    void Start()
    {
        if (dialogueLines == null || dialogueLines.Length == 0)
        {
            Debug.LogError("dialogueLines is EMPTY! Add lines in the Inspector.");
            return;
        }

        dialogueText.text = "";
        continueText.alpha = 0;
        StartCoroutine(TypeLine());
    }

    void Update()
    {
        if (canPressE && !isTyping)
        {
            // blinking animation
            continueText.alpha = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));

            if (Input.GetKeyDown(KeyCode.E))
            {
                NextLine();
            }
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.text = "";
        continueText.alpha = 0;

        foreach (char c in dialogueLines[index])
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        yield return new WaitForSeconds(waitAfterFinished);

        canPressE = true;
    }

    void NextLine()
    {
        canPressE = false;

        if (index < dialogueLines.Length - 1)
        {
            index++;
            StartCoroutine(TypeLine());
        }
        else
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}