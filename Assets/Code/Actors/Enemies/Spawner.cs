using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Settings")]
    [SerializeField] private float spawnRadius = 20f;
    [SerializeField] private float spawnInterval = 4f;

    private float _nextSpawnTime;

    void Update()
    {
        if (Time.time >= _nextSpawnTime)
        {
            SpawnEnemy();
            _nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        // Calcula un vector aleatorio alrededor del Spawner
        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
        randomDirection += transform.position;

        // Interpola la posición aleatoria con la malla de navegación horneada
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, spawnRadius, NavMesh.AllAreas))
        {
            int randomIndex = Random.Range(0, enemyPrefabs.Length);
            Instantiate(enemyPrefabs[randomIndex], hit.position, Quaternion.identity);
        }
    }
}