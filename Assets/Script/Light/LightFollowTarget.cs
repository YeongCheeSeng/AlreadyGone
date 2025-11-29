using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFollowTarget : MonoBehaviour
{
    public GameObject target;
    public bool rotateTowardsTarget = true;
    public bool FollowTarget = true;

    private Light2D light2D;

    private void Start()
    {
        light2D = GetComponent<Light2D>();
    }

    void Update()
    {
        RotateTowardsTarget();
        
        if (FollowTarget && target != null)
        {
            transform.position = new Vector3 (target.transform.position.x, transform.localPosition.y);
        }
    }

    private void RotateTowardsTarget()
    {
        if (!rotateTowardsTarget || target == null)
            return;

        Vector3 Look = transform.InverseTransformPoint(target.transform.position);
        float angle = Mathf.Atan2(Look.y, Look.x) * Mathf.Rad2Deg - 90;

        transform.Rotate(0, 0, angle);

        light2D.pointLightOuterRadius = Mathf.Abs(Look.y) + 5f;
    }
}
