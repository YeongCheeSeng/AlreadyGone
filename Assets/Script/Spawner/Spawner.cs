using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefab to Spawn")]
    public GameObject spawnPrefab;

    public float interval = 0.5f;
    private bool canSpawn = true;
    private GameObject spawnedGameObject;

    private void Start()
    {
        if (spawnPrefab != null)
            spawnPrefab.SetActive(false);
    }
    void Update()
    {
        if (canSpawn && spawnedGameObject == null)
        {
            canSpawn = false;
            StartCoroutine(SpawnRoutine());
        }
    }

    IEnumerator SpawnRoutine()
    {
        spawnedGameObject = Instantiate(spawnPrefab, transform.position, transform.rotation);
        spawnedGameObject.SetActive(true);

        Vector3 scale = spawnedGameObject.transform.localScale;
        scale.x *= Mathf.Sign(transform.localScale.x);
        spawnedGameObject.transform.localScale = scale;

        yield return new WaitForSeconds(interval);

        canSpawn = true;
    }
}