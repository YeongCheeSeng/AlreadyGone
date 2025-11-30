using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToward : MonoBehaviour
{
    public Transform target;
    public float durationFromAMoveToB = 5f;
    public AnimationCurve easeCurve;
    private float t = 0f;
    public bool destroyMeAfterArrived;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        t += Time.deltaTime / durationFromAMoveToB;
        t = Mathf.Clamp01(t);

        float eased = easeCurve.Evaluate(t);

        transform.position = Vector2.MoveTowards(transform.position, target.position, eased);

        if (destroyMeAfterArrived && Vector2.Distance(transform.position, target.position) < 0.1f)
        {
           Destroy(gameObject);
        }
    }
}
