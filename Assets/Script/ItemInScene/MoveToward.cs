using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToward : MonoBehaviour
{
    public Transform target;
    public bool waypointMovement;
    public float durationFromAMoveToB = 5f;
    public AnimationCurve easeCurve;

    private Vector3 pointA;
    private Vector3 pointB;
    private float t = 0f;
    public bool destroyMeAfterArrived;
    private bool goingToB = true;

    // Start is called before the first frame update
    void Start()
    {
        pointA = transform.position;
        if (target != null)
        { 
            pointB = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        t += Time.deltaTime / durationFromAMoveToB;
        t = Mathf.Clamp01(t);

        float eased = easeCurve.Evaluate(t);

        if (waypointMovement)
        {
            if (goingToB)
                transform.position = Vector3.Lerp(pointA, pointB, eased);
            else
                transform.position = Vector3.Lerp(pointB, pointA, eased);

            if (t >= 1f)
            {
                t = 0f;
                goingToB = !goingToB;
            }

        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, eased);

            if (destroyMeAfterArrived && Vector2.Distance(transform.position, target.position) < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }
}
