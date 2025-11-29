using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefab to Spawn")]
    public GameObject spawnPrefab;   // <-- prefab (never destroy this!)

    public float interval = 0.5f;
    private bool canSpawn = true;

    void Update()
    {
        if (canSpawn)
        {
            canSpawn = false;
            StartCoroutine(SpawnRoutine());
        }
    }

    IEnumerator SpawnRoutine()
    {
        // Spawn the prefab, not the scene object
        Instantiate(spawnPrefab, transform.position, transform.rotation);

        yield return new WaitForSeconds(interval);

        canSpawn = true;
    }
}