using System.Collections;
using UnityEngine;

public class Jiggle : MonoBehaviour
{
    [Header("Common Settings")]
    public float jiggleInterval = 0.3f;
    public float BiggerTheGameobjectBy = 1.3f;

    [Header("Position Jiggle")]
    public bool enableLeftRightJiggle;
    public bool enableUpDownJiggle;
    public float jiggleRange = 0.5f;
    public float jiggleSpeed = 5f;

    [Header("Rotation Jiggle")]
    public bool enableRotationJiggle;
    public float rotationAngle = 30f;

    private Vector2 defaultPosition;
    private Quaternion defaultRotation;
    private Vector2 defaultScale;

    private Coroutine jiggleRoutine;
    public Transform spriteGameobject;
    public bool isJiggling = false;

    public float cooldown;
    private void OnDisable()
    {
        StopJiggle();
    }

    private void Update()
    {
        if (isJiggling == false) 
        {
            isJiggling = true;
            StartJiggle();
            wait();
        }
    }

    IEnumerator wait()
    { 
        yield return new WaitForSeconds(cooldown);
        isJiggling = false;
    }

    public void StartJiggle()
    {
        StopJiggle(); // Ensure any existing jiggle is properly stopped

        Transform targetTransform = spriteGameobject != null ? spriteGameobject : transform;

        // Store initial transform state
        defaultPosition = targetTransform.position;
        defaultRotation = targetTransform.rotation;
        defaultScale = targetTransform.localScale;

        PopUp(BiggerTheGameobjectBy);
        //isJiggling = true;
        jiggleRoutine = StartCoroutine(JiggleRoutine(jiggleInterval));
    }

    public void StopJiggle()
    {
        if (jiggleRoutine != null)
        {
            StopCoroutine(jiggleRoutine);
            jiggleRoutine = null;
        }

        if (isJiggling)
        {
            ResetTransform();
            isJiggling = false;
        }
    }

    private IEnumerator JiggleRoutine(float interval)
    {
        Transform targetTransform = spriteGameobject != null ? spriteGameobject : transform;

        try
        {
            // Define positions
            Vector3 leftPos = defaultPosition + Vector2.left * jiggleRange;
            Vector3 rightPos = defaultPosition + Vector2.right * jiggleRange;
            Vector3 upPos = defaultPosition + Vector2.up * jiggleRange;

            // Define rotations
            Quaternion leftRot = Quaternion.Euler(defaultRotation.eulerAngles + new Vector3(0, 0, rotationAngle));
            Quaternion rightRot = Quaternion.Euler(defaultRotation.eulerAngles + new Vector3(0, 0, -rotationAngle));

            // First movement: Left or Up
            if (enableLeftRightJiggle)
                yield return MoveToPosition(leftPos, jiggleSpeed);
            else if (enableUpDownJiggle)
                yield return MoveToPosition(upPos, jiggleSpeed);

            if (enableRotationJiggle)
                targetTransform.rotation = leftRot;

            yield return new WaitForSeconds(interval);

            // Second movement: Right or Back to center
            if (enableLeftRightJiggle)
                yield return MoveToPosition(rightPos, jiggleSpeed);
            else if (enableUpDownJiggle)
                yield return MoveToPosition(defaultPosition, jiggleSpeed);

            if (enableRotationJiggle)
                targetTransform.rotation = rightRot;

            yield return new WaitForSeconds(interval);

            // Return to center
            yield return MoveToPosition(defaultPosition, jiggleSpeed);
            targetTransform.rotation = defaultRotation;

            yield return new WaitForSeconds(interval);
        }
        finally
        {
            ResetTransform();
            isJiggling = false;
            jiggleRoutine = null;
        }
    }

    private IEnumerator MoveToPosition(Vector3 target, float speed)
    {
        Transform targetTransform = spriteGameobject != null ? spriteGameobject : transform;

        float startTime = Time.time;
        float journeyLength = Vector3.Distance(targetTransform.position, target);
        float maxDuration = journeyLength / speed; // Prevent infinite loops

        while (Vector3.Distance(targetTransform.position, target) > 0.01f)
        {
            if (Time.time - startTime > maxDuration)
                break; // Safeguard against getting stuck

            targetTransform.position = Vector3.MoveTowards(targetTransform.position, target, Time.deltaTime * speed);
            yield return null;
        }
    }

    private void ResetTransform()
    {
        Transform targetTransform = spriteGameobject != null ? spriteGameobject : transform;

        targetTransform.position = defaultPosition;
        targetTransform.rotation = defaultRotation;
        targetTransform.localScale = defaultScale;
    }

    private void PopUp(float enlargeSize)
    {
        Transform targetTransform = spriteGameobject != null ? spriteGameobject : transform;
        targetTransform.localScale = defaultScale * enlargeSize;
    }
}