using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyFF : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    [Header("Settings")]
    [SerializeField] private float projectileSpeed = 20f;
    [SerializeField] private float combatDistance = 12f;
    [SerializeField] private float fireRate = 2f;
    [SerializeField] private float repositionCooldown = 3.5f;

    private NavMeshAgent _agent;
    private float _nextFireTime;
    private float _nextRepositionTime;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null) return;

        // Fija la mirada sin inclinar el modelo en el eje Y
        Vector3 lookPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(lookPosition);

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > combatDistance)
        {
            _agent.SetDestination(player.position);
        }
        else
        {
            if (Time.time >= _nextFireTime)
            {
                Shoot();
                _nextFireTime = Time.time + fireRate;
            }

            if (Time.time >= _nextRepositionTime)
            {
                Reposition();
                _nextRepositionTime = Time.time + repositionCooldown;
            }
        }
    }

    private void Shoot()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            EnemyProjectile projectileScript = projectile.GetComponent<EnemyProjectile>();

            if (projectileScript != null)
            {
                projectileScript.Initialize(projectileSpeed, 10);
            }
        }
    }

    private void Reposition()
    {
        // Genera un vector aleatorio dentro de una esfera de 6 metros
        Vector3 randomDirection = Random.insideUnitSphere * 6f;
        randomDirection += transform.position;

        // Valida que el punto aleatorio exista dentro de las áreas caminables
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 6f, NavMesh.AllAreas))
        {
            _agent.SetDestination(hit.position);
        }
    }
}