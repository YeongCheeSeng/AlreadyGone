using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWhenMasterDie : MonoBehaviour
{
    public GameObject Master;
    // Update is called once per frame

    void Update()
    {
        if (Master == null)
        {
            Destroy(gameObject);
        }
    }
}
