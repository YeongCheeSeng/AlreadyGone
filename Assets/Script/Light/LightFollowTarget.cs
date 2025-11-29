using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFollowTarget : MonoBehaviour
{
    public GameObject target;

    void Update()
    {
        Vector3 targetPos = target.transform.position;
        targetPos.z = transform.position.z;

        Vector2 direction = targetPos - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle = (angle + 360) % 360;

        transform.rotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, angle);
    }
}
