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
        dialogueText.text = "";
        continueText.alpha = 0;

        if (dialogueLines == null || dialogueLines.Length == 0)
        {
            canPressE = true;
            return;
        }

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