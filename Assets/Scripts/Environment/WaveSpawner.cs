using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] GameObject wavePrefab;
    [SerializeField] Transform spawnPoint;

    public bool automaticSpawn = true;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (automaticSpawn)
        {
            SpawnWave();
            yield return new WaitForSeconds(5f);
        }
    }

    public void SpawnWave()
    {
        Instantiate(wavePrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public void StopAutomaticSpawn()
    {
        automaticSpawn = false;
    }

    public void StartAutomaticSpawn()
    {
        automaticSpawn = true;
    }
}