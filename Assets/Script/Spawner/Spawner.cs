using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawnGameObject;
    private GameObject recordspawnGameObject;
    public float interval = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        recordspawnGameObject = spawnGameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnGameObject != null)
        { 
            
        }
    }

    IEnumerator Wait()
    { 
        yield return new WaitForSeconds(interval);
    }
}
